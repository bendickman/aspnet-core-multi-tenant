using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MultiTenant.Core.Constants;
using MultiTenant.Core.DTOs;
using MultiTenant.Core.Entities;
using MultiTenant.Core.Interfaces;
using MultiTenant.Core.Settings;
using MultiTenant.Infrastructure.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiTenant.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITenantService _tenantService;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ILogger<IdentityService> _logger;
        private readonly ApplicationDbContext _dbContext;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(
            UserManager<IdentityUser> userManager,
            ITenantService tenantService,
            TokenValidationParameters tokenValidationParameters,
            ILogger<IdentityService> logger,
            ApplicationDbContext dbContext,
            JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _tenantService = tenantService;
            _tokenValidationParameters = tokenValidationParameters;
            _logger = logger;
            _dbContext = dbContext;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> LoginAsync(
            string email, 
            string password)
        {
            var user = await _userManager
                .FindByEmailAsync(email);

            //the error message is very generic to ensure we don't expose any details to hackers.
            var errors = new List<string>
                {
                    "Incorrect Email or Password",
                };

            if (user is null)
            {
                AuthenticationResult
                    .Error(errors);
            }

            var userHasValidPassword = await _userManager
                .CheckPasswordAsync(user, password);

            if (!userHasValidPassword)
            {
                return AuthenticationResult
                    .Error(errors);
            }

            return await CreateAuthenticationResult(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(
            string email, 
            string password)
        {
            var existingUser = await _userManager
                .FindByEmailAsync(email);

            if (existingUser is not null)
            {
                var errors = new List<string>
                {
                    "User with this email already exists",
                };

                return AuthenticationResult
                    .Error(errors);
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email,
            };

            var createdUser = await _userManager
                .CreateAsync(newUser, password);

            if (!createdUser.Succeeded)
            {
                var errors = createdUser
                    .Errors
                    .Select(e => e.Description);

                return AuthenticationResult
                    .Error(errors);
            }

            return await CreateAuthenticationResult(newUser);
        }

        public async Task<AuthenticationResult> RefreshTokenAsync
            (string token, 
            string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken is null)
            {
                var errors = new List<string> { "Invalid Token" };

                return AuthenticationResult
                    .Error(errors);
            }

            var expiryDateUnix =
                long.Parse(validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                var errors = new List<string> { "Token hasn't expired yet" };

                return AuthenticationResult
                    .Error(errors);
            }

            var jti = validatedToken.Claims.Single(c => c.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = await _dbContext
                .RefreshTokens
                .SingleOrDefaultAsync(r => r.Id == refreshToken);

            if (storedRefreshToken is null)
            {
                var errors = new List<string> { "Refresh token does not exist" };

                return AuthenticationResult
                    .Error(errors);
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                var errors = new List<string> { "Refresh token has expired" };

                return AuthenticationResult
                    .Error(errors);
            }

            if (storedRefreshToken.IsInvalidated)
            {
                var errors = new List<string> { "Refresh token has been invalidated" };

                return AuthenticationResult
                    .Error(errors);
            }

            if (storedRefreshToken.IsUsed)
            {
                var errors = new List<string> { "Refresh token has already been used" };

                return AuthenticationResult
                    .Error(errors);
            }

            if (storedRefreshToken.JwtId != jti)
            {
                var errors = new List<string> { "Refresh token does not match the JWT" };

                return AuthenticationResult
                    .Error(errors);
            }

            storedRefreshToken.IsUsed = true;
            _dbContext.RefreshTokens.Update(storedRefreshToken);
            await _dbContext.SaveChangesAsync();

            var userId = validatedToken.Claims.Single(c => c.Type == DataConstants.Claims.UserId).Value;
            var user = await _userManager
                .FindByIdAsync(userId);

            return await CreateAuthenticationResult(user);            
        }

        private ClaimsPrincipal GetPrincipalFromToken(
            string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler
                    .ValidateToken(token, _tokenValidationParameters, out var validatedToken);

                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch (Exception ex)
            {
                _logger
                    .LogError("Error", ex);

                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                    StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AuthenticationResult> CreateAuthenticationResult(
            IdentityUser user)
        {
            var tenant = _tenantService.GetTenant();

            if (tenant is null)
            {
                throw new Exception("Invalid tenant");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(tenant.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//used to invalidate the token
                    new Claim(DataConstants.Claims.UserId, user.Id),       
                    new Claim(DataConstants.Claims.TenantId, tenant.Id),
                }),
                Expires = DateTime.UtcNow.AddSeconds(_jwtSettings.TokenExpirationInSeconds),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key) {  KeyId = tenant.Id}, SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler
                .CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                JwtId = token.Id,
                UserId = user.Id,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddDays(_jwtSettings.RefreshTokenLifetimeInDays),
            };

            await _dbContext
                .RefreshTokens
                .AddAsync(refreshToken);

            await _dbContext
                .SaveChangesAsync();

            return AuthenticationResult
                .Success(tokenHandler.WriteToken(token), refreshToken.Id);
        }
    }
}

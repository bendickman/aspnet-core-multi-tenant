using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MultiTenant.Core.DTOs;
using MultiTenant.Core.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiTenant.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ITenantService _tenantService;

        public IdentityService(
            UserManager<IdentityUser> userManager,
            ITenantService tenantService)
        {
            _userManager = userManager;
            _tenantService = tenantService;
        }

        public async Task<AuthenticationResult> LoginAsync(string email, string password)
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

            var token = GetJwtToken(user);

            return AuthenticationResult
                .Success(token);

        }

        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
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

            var token = GetJwtToken(
                newUser);

            return AuthenticationResult
                .Success(token);
        }

        private string GetJwtToken(IdentityUser newUser)
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
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//used to invalidate the token
                    new Claim("id", newUser.Id),       
                    new Claim("Tenant", tenant.Id),
                }),
                Expires = DateTime.UtcNow.AddMinutes(tenant.TokenExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key) {  KeyId = tenant.Id}, SecurityAlgorithms.HmacSha256Signature),
            };

            var securityToken = tokenHandler
                .CreateToken(tokenDescriptor);

            return tokenHandler
                .WriteToken(securityToken);
        }
    }
}

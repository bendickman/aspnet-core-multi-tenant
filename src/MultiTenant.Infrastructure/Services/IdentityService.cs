using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MultiTenant.Core.DTOs;
using MultiTenant.Core.Interfaces;
using MultiTenant.Core.Settings;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MultiTenant.Infrastructure.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityService(
            UserManager<IdentityUser> userManager,
            JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
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
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),//used to invalidate the token
                    new Claim("id", newUser.Id),
                }),
                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.TokenExpirationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var securityToken = tokenHandler
                .CreateToken(tokenDescriptor);

            return tokenHandler
                .WriteToken(securityToken);
        }
    }
}

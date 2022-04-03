using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using MultiTenant.Core.Settings;
using System.Text;

namespace MultiTenant.ApiCore.Authentication
{
    public static class AuthenticationSetup
    {
        public static void SetupAuthentication(
            this IServiceCollection services,
            TenantSettings tenantSettings)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = false,
                    ValidateAudience = false,
                    IssuerSigningKeyResolver = (string token, SecurityToken securityToken, string kid, TokenValidationParameters validationParameters) =>
                    {
                        var tenant = tenantSettings.Tenants.FirstOrDefault(t => t.Id == kid);
                        List<SecurityKey> keys = new List<SecurityKey>();
                        if (tenant is not null)
                        {
                            var key = Encoding.ASCII.GetBytes(tenant.SecretKey);
                            var signingKey = new SymmetricSecurityKey(key){ KeyId = tenant.Id };
                            keys.Add(signingKey);
                        }

                        return keys;
                    }
                };
                
            });
        }
    }
}

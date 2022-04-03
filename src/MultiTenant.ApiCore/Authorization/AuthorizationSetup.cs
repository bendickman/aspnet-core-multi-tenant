using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using MultiTenant.ApiCore.Authorization.Handlers;

namespace MultiTenant.ApiCore.Authorization
{
    public static class AuthorizationSetup
    {
        public static void SetupAuthorization(
            this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Tenant", policy => policy.Requirements.Add(new TenantRequirement()));
            });

            services.AddScoped<IAuthorizationHandler, TenantAuthorizationHandler>();
        }
    }
}

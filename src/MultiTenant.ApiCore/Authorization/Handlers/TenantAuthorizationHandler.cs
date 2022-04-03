using Microsoft.AspNetCore.Authorization;
using MultiTenant.Core.Interfaces;

namespace MultiTenant.ApiCore.Authorization.Handlers
{
    internal class TenantAuthorizationHandler : AuthorizationHandler<TenantRequirement>
    {
        private readonly ITenantService _tenantService;
        private const string _tenantClaimTypeName = "Tenant";

        public TenantAuthorizationHandler(
            ITenantService tenantService)
        {
            _tenantService = tenantService;
        }

        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            TenantRequirement requirement)
        {
            if (!context?.User.HasClaim(c => c.Type == _tenantClaimTypeName) ?? true)
            {
                return Task.CompletedTask;
            }

            var tenantId = context.User.FindFirst(c => c.Type == _tenantClaimTypeName)?.Value;
            if (string.IsNullOrEmpty(tenantId))
            {
                return Task.CompletedTask;
            }

            var currentTenant = _tenantService
                .GetTenant();

            if (!currentTenant?.Id.Equals(tenantId) ?? true)
            {
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}

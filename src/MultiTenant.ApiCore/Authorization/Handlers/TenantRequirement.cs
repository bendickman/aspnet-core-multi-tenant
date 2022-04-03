using Microsoft.AspNetCore.Authorization;

namespace MultiTenant.ApiCore.Authorization.Handlers
{
    internal class TenantRequirement : IAuthorizationRequirement
    {
        public TenantRequirement()
        {

        }
    }
}

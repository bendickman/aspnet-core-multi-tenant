using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MultiTenant.Core.Interfaces;
using MultiTenant.Core.Settings;

namespace MultiTenant.Infrastructure.Services
{
    public class TenantService 
        : ITenantService
    {
        private readonly TenantSettings _tenantSettings;
        private HttpContext _httpContext;
        private Tenant _currentTenant;

        public TenantService(
            IOptions<TenantSettings> tenantSettings, 
            IHttpContextAccessor contextAccessor)
        {
            _tenantSettings = tenantSettings.Value;
            _httpContext = contextAccessor.HttpContext;

            if (_httpContext != null)
            {
                if (_httpContext.Request.Headers.TryGetValue("tenant", out var tenantId))
                {
                    SetTenant(tenantId);
                }
                else
                {
                    throw new Exception("Invalid Tenant - please provid a valid tenant");
                }
            }
        }

        private void SetTenant(string tenantId)
        {
            _currentTenant = _tenantSettings
                .Tenants
                .FirstOrDefault(a => a.Id == tenantId);

            if (_currentTenant == null)
            {
                throw new Exception("Invalid Tenant - please provid a valid tenant");
            }

            if (string.IsNullOrEmpty(_currentTenant.ConnectionString))
            {
                SetDefaultConnectionStringToCurrentTenant();
            }
        }

        private void SetDefaultConnectionStringToCurrentTenant()
        {
            _currentTenant.ConnectionString = _tenantSettings.Defaults.ConnectionString;
        }

        public string GetConnectionString()
        {
            return _currentTenant?.ConnectionString;
        }

        public string GetDatabaseProvider()
        {
            return _tenantSettings.Defaults?.DBProvider;
        }

        public Tenant GetTenant()
        {
            return _currentTenant;
        }
    }
}

using MultiTenant.Core.Settings;

namespace MultiTenant.Core.Interfaces
{
    public interface ITenantService
    {
        public string GetDatabaseProvider();

        public string GetConnectionString();

        public Tenant GetTenant();
    }
}

namespace MultiTenant.Core.Settings
{
    public class TenantSettings
    {
        public Configuration Defaults { get; set; }

        public IList<Tenant> Tenants { get; set; }
    }
}

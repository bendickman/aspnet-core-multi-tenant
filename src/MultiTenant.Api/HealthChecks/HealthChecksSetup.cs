namespace MultiTenant.Api.HealthChecks
{
    public static class HealthChecksSetup
    {
        public static void Setup(IHealthChecksBuilder healthChecks)
        {
            
            healthChecks.AddCheck<DatabaseHealthCheck>("Tenant Database Connection");
        }
    }
}

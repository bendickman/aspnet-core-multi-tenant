using Microsoft.Extensions.Options;
using MultiTenant.Core.Settings;
using System.Collections.Generic;

namespace MultiTenant.UnitTests.Mockers.Services
{
    public class TenantSettingsData
    {
        public static IOptions<TenantSettings> CreateTenantSettings()
        {
            var tenantSettings = new TenantSettings
            {
                Defaults = new Configuration
                {
                    ConnectionString = "DefaultConnectionString",
                    DBProvider = "DefaultProvider"
                },
                Tenants = new List<Tenant>
                {
                    new Tenant
                    {
                        Id = "Tenant1",
                        ConnectionString = "Tenant1ConnectionString",
                        Name = "Tenant1Name",
                    },
                    new Tenant
                    {
                        Id = "Tenant2",
                        ConnectionString = "Tenant2ConnectionString",
                        Name = "Tenant2Name",
                    },
                    new Tenant
                    {
                        Id = "Tenant3",
                        Name = "Tenant3Name",
                    },
                }
            };

            return Options.Create(tenantSettings);  
        }
    }
}

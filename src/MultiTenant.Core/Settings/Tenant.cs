namespace MultiTenant.Core.Settings
{
    public class Tenant
    {
        public string Name { get; set; }

        public string Id { get; set; }

        public string ConnectionString { get; set; }

        public string DBProvider { get; set; }

        public string SecretKey { get; set; }
    }
}

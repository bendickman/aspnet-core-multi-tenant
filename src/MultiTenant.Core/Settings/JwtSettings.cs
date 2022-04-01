namespace MultiTenant.Core.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public int TokenExpirationInMinutes { get; set; }
    }
}

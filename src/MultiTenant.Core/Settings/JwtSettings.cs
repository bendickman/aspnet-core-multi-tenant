namespace MultiTenant.Core.Settings
{
    public class JwtSettings
    {
        public string Secret { get; set; }

        public int TokenExpirationInSeconds { get; set; }

        public int RefreshTokenLifetimeInDays { get; set; }
    }
}

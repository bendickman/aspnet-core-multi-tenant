using Microsoft.Extensions.DependencyInjection;

namespace MultiTenant.ApiCore
{
    public class ApiCoreOptions
    {
        public ApiCoreOptions(
            IApiDetails apiDetails,
            IHealthChecksBuilder healthChecksBuilder)
        {
            ApiDetails = apiDetails;
            HealthChecksBuilder = healthChecksBuilder;
        }

        public IApiDetails ApiDetails { get; set; }

        public IHealthChecksBuilder HealthChecksBuilder { get; }
    }
}

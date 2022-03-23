using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace MultiTenant.ApiCore.HealthChecks
{
    public static class HealthChecksSetup
    {
        public static void MapApplicationHealthChecks(
            this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHealthChecks("_health");
            endpoints.MapJsonHealthChecks("_health/json", allowCaching: false);
        }

        public static void MapJsonHealthChecks(
            this IEndpointRouteBuilder endpoints,
            string pattern,
            Func<HealthCheckRegistration, bool>? predicate = null,
            bool allowCaching = false)
        {
            var options = new HealthCheckOptions
            {
                ResponseWriter = HealthCheckJsonSerializer.WriteResponse,
                AllowCachingResponses = allowCaching,
            };

            if (predicate is not null)
            {
                options.Predicate = predicate;
            }

            endpoints.MapHealthChecks(pattern, options);
        }
    }
}

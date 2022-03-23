using Microsoft.Extensions.Diagnostics.HealthChecks;
using MultiTenant.Infrastructure.Persistence;
using System.Diagnostics;

namespace MultiTenant.Api.HealthChecks
{
    public class DatabaseHealthCheck : IHealthCheck
    {
        private readonly ApplicationDbContext _dbContext;

        public DatabaseHealthCheck(
            ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context, 
            CancellationToken cancellationToken = default)
        {
            var sw = new Stopwatch();
            sw.Start();

            var canConnect = await _dbContext
                .Database
                .CanConnectAsync(cancellationToken);

            sw.Stop();
            
            var data = new Dictionary<string, object>
            {
                { "Tenant Name", _dbContext.Tenant.Name },
                { "Time Taken", sw.Elapsed.ToString() }
            };

            if (canConnect)
            {
                return HealthCheckResult.Healthy(data: data);
            }

            return HealthCheckResult.Unhealthy("A connection to the database could not be established.");
        }
    }
}

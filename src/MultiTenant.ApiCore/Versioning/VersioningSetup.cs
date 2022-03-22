using Microsoft.Extensions.DependencyInjection;

namespace MultiTenant.ApiCore.Versioning
{
    public static class VersioningSetup
    {
        public static void SetupVersioning(
            this IServiceCollection services)
        {
            services.AddApiVersioning(options => {
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });
        }
    }
}

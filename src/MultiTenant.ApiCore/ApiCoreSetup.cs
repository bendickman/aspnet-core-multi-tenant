using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MultiTenant.ApiCore.HealthChecks;
using MultiTenant.ApiCore.Swagger;
using MultiTenant.ApiCore.Versioning;
using MultiTenant.ApiCore.ExceptionHandling;

namespace MultiTenant.ApiCore
{
    public static class ApiCoreSetup
    {
        public static WebApplicationBuilder SetupApiCore(
            this WebApplicationBuilder builder,
            Action<ApiCoreOptions> setup)
        {
            return Setup(builder, setup);
        }

        public static void ConfigureApiCore(this WebApplication app)
        {
            app.UseExceptionHandling();
            app.UseRouting();
            app.MapControllers();
            app.MapHealthChecks("_health");
            app.MapJsonHealthChecks("_health/json");
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseApplicationSwagger();
        }

        private static WebApplicationBuilder Setup(
            WebApplicationBuilder builder,
            Action<ApiCoreOptions> setup)
        {
            var apiDetails = new ApiDetails(string.Empty, string.Empty);
            var healthChecksBuilder = builder.Services.AddHealthChecks();

            var options = new ApiCoreOptions(apiDetails, healthChecksBuilder);
            setup(options);

            builder.Services.AddSingleton<IApiDetails>(services => apiDetails);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();

            builder.Services.SetupSwagger();
            builder.Services.SetupVersioning();

            builder.Host.SetupSerilog();

            return builder;
        }
    }
}

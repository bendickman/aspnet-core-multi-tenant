using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MultiTenant.ApiCore.HealthChecks;
using MultiTenant.ApiCore.Swagger;
using MultiTenant.ApiCore.Versioning;
using MultiTenant.ApiCore.ExceptionHandling;
using MultiTenant.ApiCore.Authentication;
using MultiTenant.Core.Settings;
using Microsoft.Extensions.Configuration;

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
            app.UseAuthentication();
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

            var settings = builder
                .Configuration
                .GetSection(nameof(JwtSettings))
                .Get<JwtSettings>();

            builder.Services.SetupAuthentication(settings);

            builder.Host.SetupSerilog();

            return builder;
        }
    }
}

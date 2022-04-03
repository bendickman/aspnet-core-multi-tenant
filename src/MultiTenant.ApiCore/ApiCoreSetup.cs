using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MultiTenant.ApiCore.HealthChecks;
using MultiTenant.ApiCore.Swagger;
using MultiTenant.ApiCore.Versioning;
using MultiTenant.ApiCore.ExceptionHandling;
using MultiTenant.ApiCore.Authentication;
using MultiTenant.Core.Settings;
using Microsoft.Extensions.Configuration;
using MultiTenant.Core.Interfaces;
using MultiTenant.Infrastructure.Services;
using MultiTenant.Core.Conveters;
using HashidsNet;
using MultiTenant.Infrastructure.Extensions;
using MultiTenant.ApiCore.Authorization;

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
            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("_health");
            app.MapJsonHealthChecks("_health/json");
            app.UseHttpsRedirection();
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

            builder.Services.AddScoped<ITenantService, TenantService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IProductConverter, ProductConverter>();
            builder.Services.AddScoped<IIdentityService, IdentityService>();

            builder.Services.AddSingleton<IHashids>(_ => new Hashids("testSalt", 11));

            builder.Services.Configure<TenantSettings>(
                builder.Configuration.GetSection(nameof(TenantSettings)));

            builder.Services.AddAndMigrateTenantDatabases(builder.Configuration);

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

            builder.Services.AddSingleton(settings);

            var tenantSettings = builder
                .Configuration
                .GetSection(nameof(TenantSettings))
                .Get<TenantSettings>();

            builder.Services.SetupAuthentication(tenantSettings);
            builder.Services.SetupAuthorization();

            builder.Host.SetupSerilog();

            return builder;
        }
    }
}

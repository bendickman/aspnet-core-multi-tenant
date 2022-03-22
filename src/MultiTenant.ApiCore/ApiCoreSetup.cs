

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using MultiTenant.ApiCore.Swagger;
using MultiTenant.ApiCore.Versioning;

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
            app.UseRouting();
            app.MapControllers();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseApplicationSwagger();
        }

        private static WebApplicationBuilder Setup(
            WebApplicationBuilder builder,
            Action<ApiCoreOptions> setup)
        {
            var apiDetails = new ApiDetails(string.Empty, string.Empty);

            var options = new ApiCoreOptions(apiDetails);
            setup(options);

            builder.Services.AddSingleton<IApiDetails>(services => apiDetails);
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddControllers();

            builder.Services.SetupSwagger();
            builder.Services.SetupVersioning();

            return builder;
        }
    }
}

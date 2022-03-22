using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MultiTenant.ApiCore.Swagger
{
    public static class SwaggerSetup
    {
        public static void SetupSwagger(
            this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            services.AddSwaggerGen(c =>
            {
                c.OperationFilter<SwaggerDefaultValues>();
                c.OperationFilter<AppendAuthorizeToSummaryOperationFilter>();
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public static void UseApplicationSwagger(
            this WebApplication app)
        {
            app.UseSwagger(setup =>
            {
                setup.SerializeAsV2 = true;
            });

            app.UseSwaggerUI(options =>
            {
                options.InjectStylesheet("");

                var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }

                options.RoutePrefix = string.Empty;
            });
        }
    }
}

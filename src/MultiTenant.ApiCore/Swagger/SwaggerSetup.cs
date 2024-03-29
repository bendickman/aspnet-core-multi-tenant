﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
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
                c.OperationFilter<AddTenantHeaderParameter>();

                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JJWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        Array.Empty<string>()
                    }
                });

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

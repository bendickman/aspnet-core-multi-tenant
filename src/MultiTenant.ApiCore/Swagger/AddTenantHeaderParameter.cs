using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MultiTenant.ApiCore.Swagger
{
    public class AddTenantHeaderParameter 
        : IOperationFilter
    {
        public void Apply(
            OpenApiOperation operation, 
            OperationFilterContext context)
        {
            if (operation.Parameters is null)
            {
                operation.Parameters = new List<OpenApiParameter>();
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "Tenant",
                In = ParameterLocation.Header,
                Description = "The tenant for the application to run under",
                Required = true,
            });
        }
    }
}

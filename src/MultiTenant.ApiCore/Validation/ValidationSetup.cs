using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MultiTenant.ApiCore.Validation
{
    public static class ValidationSetup
    {
        public static void SetupValidation(
            this IServiceCollection services)
        {
            services.AddFluentValidation(mvcConfig =>
            {
                mvcConfig.RegisterValidatorsFromAssembly(Assembly.GetEntryAssembly());
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;

namespace MultiTenant.ApiCore.ExceptionHandling
{
    public static class ExceptionHandlingSetup
    {
        public static void UseExceptionHandling(
            this WebApplication app)
        {
            app.UseExceptionHandler(error =>
            {
                error.UseMiddleware<ExceptionHandlingMiddleware>();
            });
        }
    }
}

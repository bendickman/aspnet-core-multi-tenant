namespace MultiTenant.Api.ExceptionHandling
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

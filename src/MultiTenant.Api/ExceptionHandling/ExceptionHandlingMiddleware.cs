using Microsoft.AspNetCore.Diagnostics;
using System.Net;

namespace MultiTenant.Api.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

            if (contextFeature != null)
            {
                LogExceptionToLogger(contextFeature.Error);
            }

            await _next(context);
        }

        private void LogExceptionToLogger(Exception exception)
        {
            var logger = _loggerFactory.CreateLogger<ILogger>();

            if (logger != null)
            {
                logger.LogError(exception, exception.Message);
            }
        }
    }
}

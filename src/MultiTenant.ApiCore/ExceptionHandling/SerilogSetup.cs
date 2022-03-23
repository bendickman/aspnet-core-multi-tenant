using Microsoft.AspNetCore.Builder;
using Serilog;

namespace MultiTenant.ApiCore.ExceptionHandling
{
    public static class SerilogSetup
    {
        public static void SetupSerilog(this ConfigureHostBuilder builder)
        {
            builder.UseSerilog((hostingContext, loggingConfig) =>
             {
                 loggingConfig.ReadFrom
                 .Configuration(hostingContext.Configuration);
             });
        }
    }
}

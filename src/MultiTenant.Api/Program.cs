using MultiTenant.Core.Interfaces;
using MultiTenant.Core.Settings;
using MultiTenant.Infrastructure.Services;
using MultiTenant.Infrastructure.Extensions;
using MultiTenant.ApiCore;
using MultiTenant.Api.HealthChecks;
using HashidsNet;
using MultiTenant.Core.Conveters;

var builder = WebApplication
    .CreateBuilder(args)
    .SetupApiCore(options =>
    {
        options.ApiDetails.Name = "Multi Tenant API";
        options.ApiDetails.Description = "API highlighting multi-tenancy in .NET 6";

        HealthChecksSetup.Setup(options.HealthChecksBuilder);
    });

var app = builder.Build();
app.ConfigureApiCore();
app.Run();

public partial class Program { }

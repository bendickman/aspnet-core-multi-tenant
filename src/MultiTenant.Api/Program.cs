using MultiTenant.Core.Interfaces;
using MultiTenant.Core.Settings;
using MultiTenant.Infrastructure.Services;
using MultiTenant.Infrastructure.Extensions;
using MultiTenant.ApiCore;

var builder = WebApplication
    .CreateBuilder(args)
    .SetupApiCore(options =>
    {
        options.ApiDetails.Name = "Multi Tenant API";
        options.ApiDetails.Description = "API highlighting multi-tenancy in .NET 6";
    });


builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.Configure<TenantSettings>(
    builder.Configuration.GetSection(nameof(TenantSettings)));

builder.Services.AddAndMigrateTenantDatabases(builder.Configuration);

var app = builder.Build();
app.ConfigureApiCore();
app.Run();

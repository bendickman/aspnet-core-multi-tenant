using MultiTenant.Core.Interfaces;
using MultiTenant.Core.Settings;
using MultiTenant.Infrastructure.Services;
using MultiTenant.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<ITenantService, TenantService>();
builder.Services.AddTransient<IProductService, ProductService>();

builder.Services.Configure<TenantSettings>(
    builder.Configuration.GetSection(nameof(TenantSettings)));

builder.Services.AddAndMigrateTenantDatabases(builder.Configuration);

builder.Services.AddApiVersioning(options => {
    options.ReportApiVersions = true;
});

builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

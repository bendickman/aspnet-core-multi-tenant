using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MultiTenant.Core.Settings;
using MultiTenant.Infrastructure.Persistence;

namespace MultiTenant.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAndMigrateTenantDatabases(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var settings = GetOptions<TenantSettings>(configuration, nameof(TenantSettings));

            var defaultConnectionString = settings.Defaults?.ConnectionString;
            var defaultDbProvider = settings.Defaults?.DBProvider;

            //default DB
            services.AddDbContext<ApplicationDbContext>(m => m.UseSqlServer(e => e.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            var tenants = settings.Tenants;

            foreach (var tenant in tenants)
            {
                string connectionString;

                if (string.IsNullOrEmpty(tenant.ConnectionString))
                {
                    connectionString = defaultConnectionString;
                }
                else
                {
                    connectionString = tenant.ConnectionString;
                }

                using (var serviceProvider = services.BuildServiceProvider())
                {
                    var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
                    dbContext.Database.SetConnectionString(connectionString);

                    if (dbContext.Database.GetMigrations().Count() > 0)
                    {
                        dbContext.Database.Migrate();
                    }
                }                
            }

            return services;
        }

        public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : new()
        {
            var section = configuration.GetSection(sectionName);
            var options = new T();
            section.Bind(options);
            return options;
        }

    }
}

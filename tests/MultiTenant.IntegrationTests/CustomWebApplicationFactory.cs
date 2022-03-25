using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiTenant.Infrastructure.Persistence;
using System;
using System.Linq;

namespace MultiTenant.IntegrationTests
{
    public class CustomWebApplicationFactory 
        : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(
            IWebHostBuilder builder)
        {
            builder.ConfigureServices((services) =>
            {
                var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                if (descriptor is null)
                {
                    throw new Exception("ApplicationDbContext is not registered in the service collection");
                }

                services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDbForTesting");
                });
            });
        }
    }
}

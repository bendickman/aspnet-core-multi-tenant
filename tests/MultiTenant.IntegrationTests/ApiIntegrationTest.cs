using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MultiTenant.Infrastructure.Persistence;
using MultiTenant.IntegrationTests.Implementations;
using MultiTenant.IntegrationTests.Interfaces;
using System;
using System.Linq;
using System.Net.Http;

namespace MultiTenant.IntegrationTests
{
    public abstract class ApiIntegrationTest : IDisposable
    {
        private CustomWebApplicationFactory _factory;

        protected HttpClient HttpClient { get; }

        protected IHttpRequestSender RequestSender { get; set; }

        protected readonly static Uri BaseUri = new Uri("https://testapi.com/");

        public ApiIntegrationTest()
        {
            _factory = new CustomWebApplicationFactory();
            HttpClient = _factory.CreateClient(new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
            });

            RequestSender = new HttpRequestSender(HttpClient, null);
        }
        
        public void Dispose()
        {
            _factory.Dispose();
        }
    }

    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices((services) =>
            {
                var descriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                    typeof(DbContextOptions<ApplicationDbContext>));

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

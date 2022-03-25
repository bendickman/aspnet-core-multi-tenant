using Microsoft.AspNetCore.Mvc.Testing;
using MultiTenant.IntegrationTests.Implementations;
using MultiTenant.IntegrationTests.Interfaces;
using System;
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
}

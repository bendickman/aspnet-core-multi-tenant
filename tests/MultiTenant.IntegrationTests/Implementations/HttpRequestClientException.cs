using System;

namespace MultiTenant.IntegrationTests.Implementations
{
    public class HttpRequestClientException : Exception
    {
        public HttpRequestClientException(string message) 
            : base(message)
        {
        }
    } 
}

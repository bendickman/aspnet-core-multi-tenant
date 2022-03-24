using MultiTenant.IntegrationTests.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MultiTenant.IntegrationTests.Implementations
{
    public class HttpRequestSender : IHttpRequestSender
    {
        private readonly HttpClient _httpClient;
        private readonly IAuthenticationClient _authenticationClient;
        private readonly IHttpContentSerializer _httpContentSerializer;

        public HttpRequestSender(
            HttpClient httpClient,
            IAuthenticationClient authenticationClient,
            IHttpContentSerializer httpContentSerializer = null)
        {
            _httpClient = httpClient;
            _authenticationClient = authenticationClient;
            _httpContentSerializer = httpContentSerializer ?? new HttpContentSerializer();
        }

        public async Task<HttpResponseMessage> Get(
            string url,
            object body = null,
            string correlationId = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellation = default)
        {
            var request = await CreateRequest(HttpMethod.Get, url, correlationId, headers, cancellation);

            var response = await HandleRequest(request, body, cancellation);

            return response;
        }

        public async Task<HttpResponseMessage> Post(
            string url,
            object body,
            string correlationId = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellation = default)
        {
            var request = await CreateRequest(HttpMethod.Post, url, correlationId, headers, cancellation);

            return await HandleRequest(request, body, cancellation);
        }

        public async Task<HttpResponseMessage> Put(
            string url,
            object body,
            string correlationId = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellation = default)
        {
            var request = await CreateRequest(HttpMethod.Put, url, correlationId, headers, cancellation);

            return await HandleRequest(request, body, cancellation);
        }

        public async Task<HttpResponseMessage> Delete(
            string url,
            object body = null,
            string correlationId = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellation = default)
        {
            var request = await CreateRequest(HttpMethod.Delete, url, correlationId, headers, cancellation);

            return await HandleRequest(request, body, cancellation);
        }

        private void AddCustomHeaders(
            HttpRequestMessage request, 
            IDictionary<string, string> headers)
        {
            if (!headers?.Any() ?? true)
            {
                return;
            }

            foreach (var header in headers)
            {
                request
                    .Headers
                    .Add(header.Key, header.Value);
            }
        }

        private async Task<HttpRequestMessage> CreateRequest(
            HttpMethod method,
            string url,
            string correlationId,
            IDictionary<string, string> headers = null,
            CancellationToken cancellation = default)
        {
            var request = new HttpRequestMessage(method, url);

            if (_authenticationClient != null)
            {
                await _authenticationClient.SetAuthentication(request, cancellation);
            }

            AddCustomHeaders(request, headers);

            request.Headers.Add("x-correlation", string.IsNullOrEmpty(correlationId) ? Guid.NewGuid().ToString() : correlationId);

            return request;
        }

        private async Task<HttpResponseMessage> HandleRequest(
            HttpRequestMessage request,
            object body,
            CancellationToken cancellation = default)
        {
            if (body != null)
            {
                request.Content = ToStringContent(body);
            }

            var response = await _httpClient.SendAsync(request, cancellation);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            var content = string.Empty;
            if (response.Content != null)
            {
                content = await response.Content.ReadAsStringAsync();
            }

            if (IsClientError(response))
            {
                if (string.IsNullOrWhiteSpace(content))
                {
                    throw new HttpRequestClientException(response.ReasonPhrase + "\n" + content);
                }

                return response;
            }

            throw new HttpRequestException(response.ReasonPhrase + "\n" + content);
        }

        private StringContent ToStringContent(object item)
        {
            if (item == null)
            {
                return null;
            }

            return new StringContent(
                _httpContentSerializer.Serialize(item),
                Encoding.UTF8,
                "application/json");
        }

        private bool IsClientError(HttpResponseMessage response)
        {
            return (int)response.StatusCode >= 400
                && (int)response.StatusCode <= 499;
        }

        public static bool IsServerError(HttpResponseMessage response)
        {
            return (int)response.StatusCode >= 500
                && (int)response.StatusCode <= 599;
        }
    }
}

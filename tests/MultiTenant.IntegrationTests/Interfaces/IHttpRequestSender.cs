using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MultiTenant.IntegrationTests.Interfaces
{
    public interface IHttpRequestSender
    {
        Task<HttpResponseMessage> Get(
            string url,
            object body = null,
            string correlationId = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> Post(
            string url,
            object body = null,
            string correlationId = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> Put(
            string url,
            object body = null,
            string correlationId = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default);

        Task<HttpResponseMessage> Delete(
            string url,
            object body = null,
            string correlationId = null,
            IDictionary<string, string> headers = null,
            CancellationToken cancellationToken = default);
    }
}

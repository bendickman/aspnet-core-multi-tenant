using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MultiTenant.IntegrationTests.Interfaces
{
    public interface IAuthenticationClient
    {
        Task SetAuthentication(
            HttpRequestMessage httpRequestMessage,
            CancellationToken cancellationToken = default);
    }
}

using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace MultiTenant.IntegrationTests
{
    public class ApiResponse
    {
        private readonly static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public async static Task<T> GetResponse<T>(
            HttpResponseMessage httpResponse)
        {
            httpResponse.EnsureSuccessStatusCode();

            var content = await httpResponse
                .Content
                .ReadAsStringAsync();

            return JsonSerializer
                .Deserialize<T>(content, _jsonSerializerOptions);
        }
    }
}

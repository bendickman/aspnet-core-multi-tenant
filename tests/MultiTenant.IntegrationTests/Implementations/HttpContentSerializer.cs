using MultiTenant.IntegrationTests.Interfaces;
using System.Text.Json;

namespace MultiTenant.IntegrationTests.Implementations
{
    public class HttpContentSerializer
        : IHttpContentSerializer
    {
        private readonly static JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        };

        public T Deserialize<T>(string value)
        {
            return JsonSerializer.Deserialize<T>(value, _jsonSerializerOptions);
        }

        public string Serialize(object value)
        {
            return JsonSerializer.Serialize(value, _jsonSerializerOptions);
        }
    }
}

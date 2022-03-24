namespace MultiTenant.IntegrationTests.Interfaces
{
    public interface IHttpContentSerializer
    {
        string Serialize(object value);

        T Deserialize<T>(string value);
    }
}

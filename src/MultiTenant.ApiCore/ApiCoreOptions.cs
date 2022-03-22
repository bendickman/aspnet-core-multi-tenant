namespace MultiTenant.ApiCore
{
    public class ApiCoreOptions
    {
        public ApiCoreOptions(
            IApiDetails apiDetails)
        {
            ApiDetails = apiDetails;
        }

        public IApiDetails ApiDetails { get; set; }
    }
}

namespace MultiTenant.ApiCore.Swagger
{
    public class ApiDetails : IApiDetails
    {
        public ApiDetails(
            string name,
            string description)
        {
            Name = name;
            Description = description;
        }

        public string Name { get; set; }

        public string Description { get; set; }
    }
}

namespace MultiTenant.Api.Controllers.Requests
{
    public class CreateProductRequest
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Rate { get; set; }
    }
}

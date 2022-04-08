namespace MultiTenant.Core.DTOs
{
    public class Product
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Rate { get; set; }

        public string CategoryId { get; set; }
    }
}

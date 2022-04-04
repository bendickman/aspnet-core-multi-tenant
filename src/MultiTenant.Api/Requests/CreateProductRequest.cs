using System.ComponentModel.DataAnnotations;

namespace MultiTenant.Api.Controllers.Requests
{
    public class CreateProductRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Rate { get; set; }
    }
}

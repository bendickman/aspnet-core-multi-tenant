using MultiTenant.Core.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTenant.Core.Entities
{
    [Table("Product")]
    public class Product 
        : BaseEntity
    {
        public Product(
            string name, 
            string description, 
            decimal rate,
            int categoryId)
        {
            Name = name;
            Description = description;
            Rate = rate;
            CategoryId = categoryId;
        }
        protected Product() { }

        public string Name { get; private set; }

        public string Description { get; private set; }

        public decimal Rate { get; private set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}

using System.ComponentModel.DataAnnotations.Schema;

namespace MultiTenant.Core.Entities
{
    [Table("Category")]
    public class Category
        : BaseEntity
    {
        public string Name { get; set; }

        public bool IsActive { get; set; }

        public DateTime Created { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}

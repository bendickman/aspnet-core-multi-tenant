using HashidsNet;
using MultiTenant.Core.DTOs;

namespace MultiTenant.Core.Conveters
{
    public class ProductConverter : IProductConverter
    {
        private readonly IHashids _hashids;

        public ProductConverter(IHashids hashids)
        {
            _hashids = hashids;
        }

        public Product ToDto(Entities.Product productEntity)
        {
            return new Product
            {
                Id = _hashids.Encode(productEntity.Id),
                Name = productEntity.Name,
                Description = productEntity.Description,
                Rate = productEntity.Rate,
            };
        }

        public Entities.Product ToEntity(Product productDto)
        {
            return new Entities.Product(
                productDto.Name,
                productDto.Description,
                productDto.Rate);
        }
    }
}

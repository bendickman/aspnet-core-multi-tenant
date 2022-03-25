
namespace MultiTenant.Core.Conveters
{
    public interface IProductConverter
    {
        DTOs.Product ToDto(Entities.Product productEntity);
        Entities.Product ToEntity(DTOs.Product productDto);
    }
}
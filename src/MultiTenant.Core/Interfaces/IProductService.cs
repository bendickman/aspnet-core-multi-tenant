using MultiTenant.Core.Entities;

namespace MultiTenant.Core.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateAsync(
            string name, 
            string description, 
            int rate);

        Task<Product> GetByIdAsync(Guid id);

        Task<IReadOnlyList<Product>> GetAllAsync();
    }
}

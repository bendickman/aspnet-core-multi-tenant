using Microsoft.EntityFrameworkCore;
using MultiTenant.Core.Conveters;
using MultiTenant.Core.DTOs;
using MultiTenant.Core.Interfaces;
using MultiTenant.Infrastructure.Persistence;

namespace MultiTenant.Infrastructure.Services
{
    public class ProductService 
        : IProductService
    {
        private readonly ApplicationDbContext _context;
        private readonly IProductConverter _productConverter;

        public ProductService(
            ApplicationDbContext context,
            IProductConverter productConverter)
        {
            _context = context;
            _productConverter = productConverter;
        }

        public async Task<Product> CreateAsync(
            string name, 
            string description, 
            decimal rate,
            int categoryId)
        {
            var product = new Core.Entities.Product(name, description, rate, categoryId);

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return _productConverter
                .ToDto(product);
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            var products = await _context
                .Products
                .Include(p => p.Category)
                .ToListAsync();

            return products
                .Select(p => _productConverter.ToDto(p))
                .ToList();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            var product = await _context
                .Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product is null)
            {
                return null;
            }

            return _productConverter
                .ToDto(product);
        }
    }
}

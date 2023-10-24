using Microsoft.EntityFrameworkCore;
using TektonLabs.Core.Application.Services.Products;
using TektonLabs.Core.Domain.Entities;

namespace TektonLabs.Infrastructure.DataAccess.Persistence
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDbContext _context;
        public ProductRepository(ProductDbContext context)
        {
            _context = context;
        }

        public async Task<Product> GetByIdAsync(int id) => await _context.Products.FindAsync(id);

        public async Task<Product> InsertAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<int> UpdateAsync(Product product)
        {
            _context.ChangeTracker.Clear();
            _context.Entry(product).State = EntityState.Modified;
            return await _context.SaveChangesAsync();
        }
    }
}

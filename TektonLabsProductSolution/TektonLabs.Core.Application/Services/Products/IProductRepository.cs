using TektonLabs.Core.Domain.Entities;

namespace TektonLabs.Core.Application.Services.Products
{
    public interface IProductRepository
    {
        Task<Product> GetByIdAsync(int id);

        Task<Product> InsertAsync(Product product);
    }
}

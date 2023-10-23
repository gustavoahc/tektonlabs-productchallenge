using FluentValidation.Results;
using TektonLabs.Core.Domain.Common;
using TektonLabs.Core.Domain.Entities;

namespace TektonLabs.Core.Application.Services.Products
{
    public interface IProductService
    {
        Task<Product> GetProductAsync(int id);

        Task<Product> InsertProductAsync(Product product);

        List<ValidationFailure> GetValidationErrors();

        List<StatusData> GetStatusData();

        Task<bool> UpdateProductAsync(Product product);
    }
}

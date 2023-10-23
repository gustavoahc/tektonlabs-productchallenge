using FluentValidation.Results;
using TektonLabs.Core.Application.Validators;
using TektonLabs.Core.Domain.Common;
using TektonLabs.Core.Domain.Entities;

namespace TektonLabs.Core.Application.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly ProductValidator _validator;
        private List<ValidationFailure>? _validationErrors;

        public ProductService(IProductRepository repository, ProductValidator validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task<Product> GetProductAsync(int id) => await _repository.GetByIdAsync(id);

        public async Task<Product> InsertProductAsync(Product product)
        {
            _validationErrors = null;
            var validation = await _validator.ValidateAsync(product);
            if (!validation.IsValid)
            {
                _validationErrors = validation.Errors;
                return null;
            }

            Product insertResult = await _repository.InsertAsync(product);
            return insertResult;
        }

        public List<ValidationFailure> GetValidationErrors() => _validationErrors;

        public List<StatusData> GetStatusData()
        {
            List<StatusData> statusData = new List<StatusData>();
            statusData.Add(new StatusData { Id = 0, Name = "Inactive" });
            statusData.Add(new StatusData { Id = 1, Name = "Active" });

            return statusData;
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            bool result = false;
            _validationErrors = null;

            var validation = await _validator.ValidateAsync(product);
            if (!validation.IsValid)
            {
                _validationErrors = validation.Errors;
                return result;
            }

            int resultUpdate = await _repository.UpdateAsync(product);
            if (resultUpdate > 0)
            {
                result = true;
            }

            return result;
        }
    }
}

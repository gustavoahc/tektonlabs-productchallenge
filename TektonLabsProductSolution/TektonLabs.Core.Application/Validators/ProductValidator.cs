using FluentValidation;
using TektonLabs.Core.Domain.Entities;

namespace TektonLabs.Core.Application.Validators
{
    public class ProductValidator : AbstractValidator<Product>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("Please enter {PropertyName}")
                .Length(1, 100).WithMessage("{PropertyName} cannot exceed 100 characters");

            RuleFor(p => p.Status)
                .InclusiveBetween(0, 1).WithMessage("{PropertyName} must be 0 or 1");

            RuleFor(p => p.Stock)
                .NotEmpty().WithMessage("Please enter {PropertyName}")
                .InclusiveBetween(1, int.MaxValue);

            RuleFor(p => p.Price)
                .NotEmpty().WithMessage("Please enter {PropertyName}")
                .PrecisionScale(12, 2, true).WithMessage("Value not allowed");
        }
    }
}

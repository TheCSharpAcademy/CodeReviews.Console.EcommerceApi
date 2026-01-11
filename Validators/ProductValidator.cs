using ECommerceApp.RyanW84.Data.Models;
using FluentValidation;

namespace ECommerceApp.RyanW84.Validators;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(100)
            .WithMessage("Product name must not exceed 100 characters.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Product description is required.")
            .MaximumLength(500)
            .WithMessage("Product description must not exceed 500 characters.");

        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Product price must be greater than 0.");

        RuleFor(x => x.Stock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Stock quantity cannot be negative.");

        RuleFor(x => x.CategoryId).GreaterThan(0).WithMessage("Category is required.");

        // Category navigation property is optional for updates
    }
}

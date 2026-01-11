using ECommerceApp.RyanW84.Data.Models;
using FluentValidation;

namespace ECommerceApp.RyanW84.Validators;

public class SaleItemValidator : AbstractValidator<SaleItem>
{
    public SaleItemValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Product is required for each sale item.");

        RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
    }
}

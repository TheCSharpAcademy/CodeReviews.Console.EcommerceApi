using ECommerceApp.RyanW84.Data.Models;
using FluentValidation;

namespace ECommerceApp.RyanW84.Validators;

public class SaleValidator : AbstractValidator<Sale>
{
    public SaleValidator(IValidator<SaleItem> saleItemValidator)
    {
        RuleFor(x => x.CustomerName)
            .NotEmpty()
            .WithMessage("Customer name is required.")
            .MaximumLength(100)
            .WithMessage("Customer name must not exceed 100 characters.");

        RuleFor(x => x.CustomerEmail)
            .NotEmpty()
            .WithMessage("Customer email is required.")
            .MaximumLength(100)
            .WithMessage("Customer email must not exceed 100 characters.")
            .EmailAddress()
            .WithMessage("Customer email must be a valid email address.");

        RuleFor(x => x.CustomerAddress)
            .NotEmpty()
            .WithMessage("Customer address is required.")
            .MaximumLength(200)
            .WithMessage("Customer address must not exceed 200 characters.");

        RuleFor(x => x.SaleDate).NotEmpty().WithMessage("Sale date is required.");

        RuleFor(x => x.TotalAmount)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Total amount cannot be negative.");

        RuleFor(x => x.SaleItems)
            .NotNull()
            .WithMessage("Sale items are required.")
            .NotEmpty()
            .WithMessage("A sale must contain at least one item.");

        RuleForEach(x => x.SaleItems).SetValidator(saleItemValidator);
    }
}

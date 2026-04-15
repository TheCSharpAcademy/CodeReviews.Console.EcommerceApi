using Ecommerce.Core.DTOs.Sale;
using FluentValidation;

namespace Ecommerce.Core.Validators.Sales;

public class CreateSaleDtoValidator : AbstractValidator<CreateSaleDto>
{
    public CreateSaleDtoValidator()
    {
        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Sale must have at least one item.");
        RuleForEach(x => x.Items)
            .SetValidator(new CreateSaleItemDtoValidator());
    }
}

public class CreateSaleItemDtoValidator : AbstractValidator<CreateSaleItemDto>
{
    public CreateSaleItemDtoValidator()
    {
        RuleFor(x => x.ProductId)
            .GreaterThan(0)
            .WithMessage("Product Id must be greater than zero.");
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}
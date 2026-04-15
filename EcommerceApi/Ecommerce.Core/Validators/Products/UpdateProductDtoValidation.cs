using Ecommerce.Core.DTOs.Product;
using Ecommerce.Core.Models;
using FluentValidation;

namespace Ecommerce.Core.Validators.Products;

public class UpdateProductDtoValidation : AbstractValidator<UpdateProductDto>
{
    public UpdateProductDtoValidation()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Product name is required.")
            .MaximumLength(Product.MaxNameLength)
            .WithMessage($"Product name cannot exceed {Product.MaxNameLength} characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(Product.MaxDescriptionLength)
            .WithMessage($"Product description cannot exceed {Product.MaxDescriptionLength} characters.");
        
        RuleFor(x => x.Price)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.");
        
        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity must be greater than or equal to zero.");
        
        RuleFor(x => x.CategoryId)
            .GreaterThan(0)
            .WithMessage("A valid Category ID is required.");
        
        RuleFor(x => x.ImageUrl)
            .Must(url => Uri.IsWellFormedUriString(url, UriKind.Absolute))
            .When(x => !string.IsNullOrEmpty(x.ImageUrl))
            .WithMessage("If an Image URL is provided, it must be a valid web address.");
    }
}
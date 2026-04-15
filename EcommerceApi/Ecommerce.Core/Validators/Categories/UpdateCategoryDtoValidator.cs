using Ecommerce.Core.DTOs.Category;
using Ecommerce.Core.Models;
using FluentValidation;

namespace Ecommerce.Core.Validators.Categories;

public class UpdateCategoryDtoValidator : AbstractValidator<UpdateCategoryDto>
{
    public UpdateCategoryDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Category name is required.")
            .MaximumLength(Category.MaxNameLength)
            .WithMessage($"Category name cannot exceed {Category.MaxNameLength} characters.");
        
        RuleFor(x => x.Description)
            .MaximumLength(Category.MaxDescriptionLength)
            .WithMessage($"Category description cannot exceed {Category.MaxDescriptionLength} characters.");
    }
}
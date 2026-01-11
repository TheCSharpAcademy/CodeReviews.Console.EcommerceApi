using ECommerceApp.RyanW84.Data.DTO;
using FluentValidation;

namespace ECommerceApp.RyanW84.Validators;

public class CategoryQueryParametersValidator : AbstractValidator<CategoryQueryParameters>
{
    private static readonly string[] AllowedSortColumns = ["name", "createdat"];

    public CategoryQueryParametersValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page must be greater than zero.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 32)
            .WithMessage("Page size must be between 1 and 32.");

        RuleFor(x => x.Search)
            .MaximumLength(100)
            .WithMessage("Search term must not exceed 100 characters.");

        RuleFor(x => x.SortDirection)
            .Must(BeValidSortDirection)
            .When(x => !string.IsNullOrWhiteSpace(x.SortDirection))
            .WithMessage("Sort direction must be either 'asc' or 'desc'.");

        RuleFor(x => x.SortBy)
            .Must(sortBy => AllowedSortColumns.Contains(sortBy!.ToLowerInvariant()))
            .When(x => !string.IsNullOrWhiteSpace(x.SortBy))
            .WithMessage(
                $"Sort by must be one of the following values: {string.Join(", ", AllowedSortColumns)}."
            );
    }

    private static bool BeValidSortDirection(string? direction) =>
        direction is not null
        && (
            direction.Equals("asc", StringComparison.OrdinalIgnoreCase)
            || direction.Equals("desc", StringComparison.OrdinalIgnoreCase)
        );
}

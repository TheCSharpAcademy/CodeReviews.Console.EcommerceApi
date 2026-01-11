using ECommerceApp.RyanW84.Data.DTO;
using FluentValidation;

namespace ECommerceApp.RyanW84.Validators;

public class SaleQueryParametersValidator : AbstractValidator<SaleQueryParameters>
{
    private static readonly string[] AllowedSortColumns =
    [
        "saledate",
        "totalamount",
        "customername",
    ];

    public SaleQueryParametersValidator()
    {
        RuleFor(x => x.Page).GreaterThan(0).WithMessage("Page must be greater than zero.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100)
            .WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.StartDate)
            .LessThanOrEqualTo(x => x.EndDate!.Value)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("Start date must be before or equal to end date.");

        RuleFor(x => x.CustomerName)
            .MaximumLength(100)
            .WithMessage("Customer name must not exceed 100 characters.");

        RuleFor(x => x.CustomerEmail)
            .MaximumLength(100)
            .EmailAddress()
            .When(x => !string.IsNullOrWhiteSpace(x.CustomerEmail))
            .WithMessage("Customer email must be valid and no longer than 100 characters.");

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

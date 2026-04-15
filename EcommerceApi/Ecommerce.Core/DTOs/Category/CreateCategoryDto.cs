namespace Ecommerce.Core.DTOs.Category;

public record CreateCategoryDto
{
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
}
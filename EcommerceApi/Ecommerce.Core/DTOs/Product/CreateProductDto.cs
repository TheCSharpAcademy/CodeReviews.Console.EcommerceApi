namespace Ecommerce.Core.DTOs.Product;

public record CreateProductDto
{
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public int Quantity { get; init; } = 1;
    public string? Description { get; init; }
    public string? ImageUrl { get; init; }
    public int CategoryId { get; init; }
}
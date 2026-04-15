namespace Ecommerce.Core.DTOs.Sale;

public record CreateSaleDto
{
    public List<CreateSaleItemDto> Items { get; init; } = new();
}

public record CreateSaleItemDto
{
    public int ProductId { get; init; }
    public int Quantity { get; init; }
}
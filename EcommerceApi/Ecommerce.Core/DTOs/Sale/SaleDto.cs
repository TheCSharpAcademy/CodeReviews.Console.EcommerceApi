namespace Ecommerce.Core.DTOs.Sale;

public record SaleDto
{
    public int Id { get; init; }
    public DateTime CreationDate { get; init; }
    public decimal TotalPrice { get; init; }
    public List<SaleItemDto> Items { get; init; } = new();
}

public record SaleItemDto
{
    public int ProductId { get; init; }
    public string? ProductName { get; init; } = string.Empty;
    public decimal UnitPrice { get; init; }
    public int Quantity { get; init; }
}
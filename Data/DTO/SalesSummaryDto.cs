namespace ECommerceApp.RyanW84.Data.DTO;

public sealed class SalesSummaryDto
{
    public string ProductName { get; init; } = null!;
    public string CategoryName { get; init; } = null!;
    public int TotalQuantitySold { get; init; }
    public decimal TotalRevenue { get; init; }
    public DateTime LastSaleDate { get; init; }
}

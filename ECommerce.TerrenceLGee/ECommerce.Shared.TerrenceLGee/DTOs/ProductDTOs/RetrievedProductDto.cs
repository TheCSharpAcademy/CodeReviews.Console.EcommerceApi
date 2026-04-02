namespace ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;

public class RetrievedProductDto
{
    public int Id { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int StockQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int DiscountPercentage { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsInStock { get; set; }
}
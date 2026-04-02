namespace ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;

public class ProductQueryParams : QueryStringParams
{
    public decimal? MinUnitPrice { get; set; }
    public decimal? MaxUnitPrice { get; set; }
    public int? MinStockQuantity { get; set; }
    public int? MaxStockQuantity { get; set; }
    public int? MinDiscountPercentage { get; set; }
    public int? MaxDiscountPercentage { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public string? Description { get; set; }
    public bool? InStock { get; set; }
    public bool? IsDeleted { get; set; }
    public bool IsValidUnitPriceRange => MaxUnitPrice >= MinUnitPrice;
    public bool IsValidStockQuantityRange => MaxStockQuantity >= MinStockQuantity;
    public bool IsValidDiscountPercentageRange => MaxDiscountPercentage >= MinDiscountPercentage;
}
using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;

public class ProductData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("categoryName")]
    public string CategoryName { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string? Description { get; set; }

    [JsonPropertyName("stockQuantity")]
    public int StockQuantity { get; set; }

    [JsonPropertyName("unitPrice")]
    public decimal UnitPrice { get; set; }

    [JsonPropertyName("discountPercentage")]
    public int DiscountPercentage { get; set; }

    [JsonPropertyName("isInStock")]
    public bool IsInStock { get; set; }

    [JsonPropertyName("imageUrl")]
    public string? ImageUrl { get; set; }

    public string? ErrorMessage { get; set; }
}
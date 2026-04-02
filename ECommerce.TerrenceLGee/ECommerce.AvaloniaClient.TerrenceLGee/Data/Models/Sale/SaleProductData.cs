using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;

public class SaleProductData
{
    [JsonPropertyName("saleId")]
    public int SaleId { get; set; }

    [JsonPropertyName("productId")]
    public int ProductId { get; set; }

    [JsonPropertyName("productName")]
    public string ProductName { get; set; } = string.Empty;

    [JsonPropertyName("quantity")]
    public int Quantity { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("discount")]
    public double Discount { get; set; }

    [JsonPropertyName("totalPrice")]
    public double TotalPrice { get; set; }
}
using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;

public class ProductRoot : Root
{
    [JsonPropertyName("data")]
    public ProductData? Data { get; set; }
}
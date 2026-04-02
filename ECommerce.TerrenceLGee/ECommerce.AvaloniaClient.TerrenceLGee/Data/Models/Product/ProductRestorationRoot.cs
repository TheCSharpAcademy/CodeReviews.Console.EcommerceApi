using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;

public class ProductRestorationRoot : Root
{
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}

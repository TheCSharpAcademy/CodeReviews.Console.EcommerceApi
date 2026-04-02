using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;

public class ProductAdminRoot : Root
{
    [JsonPropertyName("data")]
    public ProductAdminData? Data { get; set; }
}
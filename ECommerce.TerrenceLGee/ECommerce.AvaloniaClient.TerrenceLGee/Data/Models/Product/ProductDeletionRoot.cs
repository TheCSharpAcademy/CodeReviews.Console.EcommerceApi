using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Product;

public class ProductDeletionRoot : Root
{
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}

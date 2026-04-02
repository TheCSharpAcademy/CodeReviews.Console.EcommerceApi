using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;

public class SaleRoot : Root
{
    [JsonPropertyName("data")]
    public SaleData? Data { get; set; }
}
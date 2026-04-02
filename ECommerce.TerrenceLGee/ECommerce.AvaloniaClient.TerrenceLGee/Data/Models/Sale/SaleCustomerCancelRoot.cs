using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;

public class SaleCustomerCancelRoot : Root
{
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}
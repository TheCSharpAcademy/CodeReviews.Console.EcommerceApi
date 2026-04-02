using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Customer;

public class CustomerRoot : Root
{
    [JsonPropertyName("data")]
    public CustomerData? Data { get; set; }
}
using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;

public class AddressRoot : Root
{
    [JsonPropertyName("data")]
    public AddressData? Data { get; set; }
}
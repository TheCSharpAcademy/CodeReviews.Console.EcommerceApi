using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;

public class AddressDeletionRoot : Root
{
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}

using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Auth;

public class RegistrationRoot : Root
{
    [JsonPropertyName("data")]
    public string? Data { get; set; }
}
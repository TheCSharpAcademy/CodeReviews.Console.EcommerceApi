using System.Text.Json.Serialization;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Address;

public class AddressData
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("customerId")]
    public string? CustomerId { get; set; }

    [JsonPropertyName("customerName")]
    public string? CustomerName { get; set; }

    [JsonPropertyName("addressLine1")]
    public string AddressLine1 { get; set; } = string.Empty;

    [JsonPropertyName("addressLine2")]
    public string? AddressLine2 { get; set; }

    [JsonPropertyName("city")]
    public string City { get; set; } = string.Empty;

    [JsonPropertyName("state")]
    public string State { get; set; } = string.Empty;

    [JsonPropertyName("postalCode")]
    public string PostalCode { get; set; } = string.Empty;

    [JsonPropertyName("country")]
    public string Country { get; set; } = string.Empty;

    [JsonPropertyName("isBillingAddress")]
    public bool IsBillingAddress { get; set; }

    [JsonPropertyName("isShippingAddress")]
    public bool IsShippingAddress { get; set; }

    public string? ErrorMessage { get; set; }
}
namespace ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;

public class RetrievedAddressDto
{
    public int Id { get; set; }
    public string? CustomerId { get; set; }
    public string? CustomerName { get; set; }
    public string AddressLine1 { get; set; } = string.Empty;
    public string? AddressLine2 { get; set; }
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public bool IsBillingAddress { get; set; }
    public bool IsShippingAddress { get; set; }
}
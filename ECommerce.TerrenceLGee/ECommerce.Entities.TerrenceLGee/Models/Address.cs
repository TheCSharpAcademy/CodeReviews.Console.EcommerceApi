using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Entities.TerrenceLGee.Models;

public class Address
{
    public int Id { get; set; }
    public string? CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public ApplicationUser? Customer { get; set; }

    [Required(ErrorMessage = "Address Line 1 is required.")]
    [StringLength(100, ErrorMessage = "Address Line 1 cannot exceed 100 characters.")]
    public string AddressLine1 { get; set; } = string.Empty;

    [StringLength(100, ErrorMessage = "Address Line 2 cannot exceed 100 characters.")]
    public string? AddressLine2 { get; set; }

    [Required(ErrorMessage = "City is required.")]
    [StringLength(50, ErrorMessage = "City cannot exceed 50 characters.")]
    public string City { get; set; } = string.Empty;

    [Required(ErrorMessage = "State is required.")]
    [StringLength(50, ErrorMessage = "State cannot exeed 50 characters.")]
    public string State { get; set; } = string.Empty;

    [Required(ErrorMessage = "Postal Code is required.")]
    [RegularExpression(@"\d{4,6}$", ErrorMessage = "Invalid Postal Code.")]
    public string PostalCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Country is required.")]
    [StringLength(50, ErrorMessage = "Country cannot exceed 50 characters.")]
    public string Country { get; set; } = string.Empty;

    [Required(ErrorMessage = "It is required to specify if this address is the Billing Address.")]
    public bool IsBillingAddress { get; set; }

    [Required(ErrorMessage = "It is required to specify if this address is the Shipping Address.")]
    public bool IsShippingAddress { get; set; }
}
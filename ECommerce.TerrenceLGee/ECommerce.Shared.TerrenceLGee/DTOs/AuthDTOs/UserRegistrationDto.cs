using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;

public class UserRegistrationDto
{
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(25, ErrorMessage = "First name cannot be greater than 25 characters.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(25, ErrorMessage = "Last name cannot be greater than 25 characters.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Date of birth is required.")]
    public DateOnly DateOfBirth { get; set; }

    [Required(ErrorMessage = "Billing address is required.")]
    public CreateAddressDto? BillingAddress { get; set; }

    [Required(ErrorMessage = "Shipping address is required.")]
    public CreateAddressDto? ShippingAddress { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password length must be greater than or equal to 8 characters.")]
    public string Password { get; set; } = string.Empty;
}
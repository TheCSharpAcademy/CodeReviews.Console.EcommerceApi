using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;

public class UserLoginDto
{
    [Required(ErrorMessage = "Email address is required.")]
    [EmailAddress(ErrorMessage = "Provided email address is invalid.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [MinLength(8, ErrorMessage = "Password length must be greater than or equal to 8 characters.")]
    public string Password { get; set; } = string.Empty;
}
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;

public class UserResetPasswordDto
{
    [Required(ErrorMessage = "Email address is required.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Previous password is required.")]
    public string OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "New password is required.")]
    public string NewPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirmation password is required.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

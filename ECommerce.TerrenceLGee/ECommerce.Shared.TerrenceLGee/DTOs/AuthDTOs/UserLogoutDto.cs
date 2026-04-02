using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.AuthDTOs;

public class UserLogoutDto
{
    [Required(ErrorMessage = "UserId is required.")]
    public string UserId { get; set; } = string.Empty;
}
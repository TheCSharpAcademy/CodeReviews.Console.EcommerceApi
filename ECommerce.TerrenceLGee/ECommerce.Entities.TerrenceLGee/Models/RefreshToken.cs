using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerce.Entities.TerrenceLGee.Models;

public class RefreshToken
{
    public int Id { get; set; }
    public required string Token { get; set; }
    public required string JwtId { get; set; } = string.Empty;
    public required DateTime CreatedAt { get; set; }
    public required DateTime Expires { get; set; }
    public bool IsRevoked { get; set; } = false;
    public DateTime? RevokedAt { get; set; }

    [Required(ErrorMessage = "UserId is required.")]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey("UserId")]
    public ApplicationUser? User { get; set; } = default;
}
using Microsoft.AspNetCore.Identity;

namespace ECommerce.Entities.TerrenceLGee.Models;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public DateOnly RegistrationDate { get; set; }
    public ICollection<Sale> Sales { get; set; } = [];
    public ICollection<RefreshToken> RefreshTokens { get; set; } = [];
    public ICollection<Address> Addresses { get; set; } = [];
}
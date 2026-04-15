using Microsoft.AspNetCore.Identity;

namespace Ecommerce.Core.Models;

public class ApplicationUser : IdentityUser
{
    public string? Address { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Enums;

public enum AdminMenu
{
    [Display(Name = "Categories")]
    Categories,
    [Display(Name = "Products")]
    Products,
    [Display(Name = "Customers")]
    Customers,
    [Display(Name = "Logout")]
    Logout
}

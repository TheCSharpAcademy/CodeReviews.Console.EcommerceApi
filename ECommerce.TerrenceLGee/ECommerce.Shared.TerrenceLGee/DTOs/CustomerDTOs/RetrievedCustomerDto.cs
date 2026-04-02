using ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;

namespace ECommerce.Shared.TerrenceLGee.DTOs.CustomerDTOs;

public class RetrievedCustomerDto
{
    public string CustomerId { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string EmailAddress { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public DateOnly RegistrationDate { get; set; }
}
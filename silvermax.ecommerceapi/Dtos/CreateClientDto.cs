using System.ComponentModel.DataAnnotations;

namespace silvermax.ecommerceapi.Dtos;

public class CreateClientDto
{
    public required string Name { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    [Phone]
    public required string PhoneNumber { get; set; }
}

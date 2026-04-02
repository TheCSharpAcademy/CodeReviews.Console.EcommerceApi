using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.AddressDTOs;

public class AddressIdDto
{
    [Required(ErrorMessage = "Address Id is required.")]
    public int Id { get; set; }
    
    public string? CustomerId { get; set; }
}
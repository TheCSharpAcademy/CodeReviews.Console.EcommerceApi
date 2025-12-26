using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JafnaEcommerceApi.Models.DTOs.ProductDTOs;

public class ProductCreateDto
{
    [Required]
    public string Name { get; set; }

    [Required]
    public string Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    public string? Image { get; set; }

    [Required]
    public int CategoryId { get; set; }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JafnaEcommerceApi.Models.DTOs.ProductDTOs;

public class ProductUpdateDto
{
    public string? Name { get; set; }

    public string? Description { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal? Price { get; set; }

    public string? Image { get; set; }
}

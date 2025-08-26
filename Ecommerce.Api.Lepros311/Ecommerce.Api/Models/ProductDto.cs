using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Models;

public class ProductDto
{
    public int ProductId { get; set; }

    [Required]
    public string? ProductName { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public string? Category { get; set; }
}

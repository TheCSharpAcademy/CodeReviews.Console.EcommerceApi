using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Api.Models;

public class WriteProductDto
{
    [Required]
    public string ProductName { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public int CategoryId { get; set; }
}

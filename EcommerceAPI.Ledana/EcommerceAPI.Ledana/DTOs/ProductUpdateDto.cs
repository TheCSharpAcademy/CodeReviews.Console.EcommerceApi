using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Ledana.DTOs
{
    public class ProductUpdateDto
    {
        [Required]
        public string? Name { get; set; } = null!;
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Stock must be greater than 0.")]
        public int? Stock { get; set; }
        [Required]
        public int? CategoryId { get; set; }
    }
}

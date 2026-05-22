using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Ledana.DTOs
{
    public class CategoryDto
    {
        [Required]
        public string Name { get; set; } = null!;
    }
}

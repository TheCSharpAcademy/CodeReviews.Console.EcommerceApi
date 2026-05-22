using System.ComponentModel.DataAnnotations;

namespace EcommerceAPI.Ledana.DTOs
{
    public class SaleDto
    {
        [Required]
        public DateTime? Date { get; set; }
        [Required]
        public List<SaleProductDto>? SaleProducts { get; set; } = [];
    }
}

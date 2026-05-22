
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace EcommerceAPI.Ledana.DTOs
{
    //dto for saleproduct when in use to create a new one
    public class SaleProductDto
    {
        [Required]
        public int? ProductsId { get; set; }
        [Required]
        public int? Quantity { get; set; }
        [Required]
        public decimal? Discount { get; set; }


    }
    //dto for saleproduct to get in sale
    public class SaleProductListDto
    {
        public string ProductName { get; set; } = null!;
        public string CategoryName { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal UnitPriceAtSale { get; set; }
        public decimal TotalPrice { get; set; }
    }

    //dto for saleproduct when in use to view one
    public class SaleProductViewDto
    {
        public int SaleId { get; set; }
        public DateTime Date { get; set; }
        public List<SaleProductListDto> Products { get; set; } = [];
        public decimal TotalPrice { get; set; } //= Products.Sum(p => p.TotalPrice);
    }
}

namespace EcommerceAPI.Ledana.Models
{
    public class SaleProduct
    {
        public int ProductsId { get; set; }
        public Product Product { get; set; } = null!;
        public int SalesId { get; set; }
        public Sale Sale { get; set; } = null!;
        public int Quantity { get; set; }
        public decimal Discount { get; set; }
        public decimal UnitPriceAtSale { get; set; }
        public decimal TotalPrice { get; set; }

    }
}

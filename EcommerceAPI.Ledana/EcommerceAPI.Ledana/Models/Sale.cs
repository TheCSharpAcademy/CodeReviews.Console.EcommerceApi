namespace EcommerceAPI.Ledana.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public List<Product> Products { get; set; } = [];

        public List<SaleProduct> SaleProducts { get; set; } = [];
        
    }

    //tried creating a view in db to have totalprice for each sale
    public class SaleWithTotal
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public decimal TotalPrice { get; set; }
    }
}

using EcommerceAPI.Ledana.Interfaces;

namespace EcommerceAPI.Ledana.Models
{
    public class Product : ISoftDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public decimal Price { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<Sale> Sales { get; set; } = [];
        public List<SaleProduct> SaleProducts { get; set; } = [];
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
    }
}

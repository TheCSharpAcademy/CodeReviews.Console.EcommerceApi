using EcommerceAPI.Ledana.Interfaces;

namespace EcommerceAPI.Ledana.Models
{
    public class Category : ISoftDeletable
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public List<Product> Products { get; set; } = [];
        public bool IsDeleted { get; set; }
        public DateTime? DeletedOnUtc { get; set; }
    }
}

namespace EcommerceApi.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int CategoryId { get; set; }
    public bool IsDeleted { get; set; }
    public Category Category { get; set; } = null!;
    public ICollection<SaleItem> SaleItems { get; set; } = [];
}

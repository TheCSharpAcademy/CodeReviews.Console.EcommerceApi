namespace Sills.GolfShop.eCommerceAPI.Models;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public int QuantityInStock { get; set; }
    public DateTime? DeletedAt { get; set; }

    public List<ProductSales> ProductSales { get; } = [];
    public int CategoryId { get; internal set; }
}

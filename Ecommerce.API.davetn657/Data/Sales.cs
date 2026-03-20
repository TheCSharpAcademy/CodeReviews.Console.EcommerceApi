namespace Ecommerce.API.davetn657.Data;

public class Sale
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    public List<Product> Products { get; set; } = [];
}
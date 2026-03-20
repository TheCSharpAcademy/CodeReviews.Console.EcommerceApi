namespace Ecommerce.API.davetn657.Data;
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public List<Sale> Sales { get; set; } = [];
    public string Category { get; set; } = string.Empty;
}
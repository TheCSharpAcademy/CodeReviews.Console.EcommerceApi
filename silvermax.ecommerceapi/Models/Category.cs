namespace silvermax.ecommerceapi.Models;

public class Category
{
    public Guid CategoryId { get; set; }
    public required string Name { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}

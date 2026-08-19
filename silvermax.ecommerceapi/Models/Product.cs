namespace silvermax.ecommerceapi.Models;

public class Product
{
    public Guid ProductId { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
    public string? ImgUrl { get; set; }
    public Category Category { get; set; } = null!;
    public Guid CategoryId { get; set; }
}

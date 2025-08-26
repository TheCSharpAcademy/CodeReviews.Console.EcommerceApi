namespace Ecommerce.Api.Models;

public class Product
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal Price { get; set; }

    public int CategoryId { get; set; }

    public Category Category { get; set; }

    public List<LineItem> LineItems { get; } = [];

    public bool IsDeleted { get; set; } = false;
}

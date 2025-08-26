namespace Ecommerce.Api.Models;

public class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public List<Product> Products { get; } = [];

    public bool IsDeleted { get; set; } = false;

    public override string ToString() => $"{CategoryName}";
}

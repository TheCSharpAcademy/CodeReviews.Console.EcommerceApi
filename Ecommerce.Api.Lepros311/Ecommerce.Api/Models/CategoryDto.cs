namespace Ecommerce.Api.Models;

public class CategoryDto
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; }

    public List<ProductDto> Products { get; set; } = [];
}

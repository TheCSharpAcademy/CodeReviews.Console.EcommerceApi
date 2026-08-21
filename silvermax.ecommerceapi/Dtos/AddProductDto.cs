using silvermax.ecommerceapi.Models;

namespace silvermax.ecommerceapi.Dtos;

public class AddProductDto
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public double Price { get; set; }
    public string? ImgUrl { get; set; }
    public string? Category { get; set; }
}

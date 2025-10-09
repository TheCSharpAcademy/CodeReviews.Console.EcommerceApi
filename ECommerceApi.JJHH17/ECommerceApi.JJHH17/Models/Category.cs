namespace ECommerceApi.JJHH17.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }

    public record CreateCategoryDto(string Name);

    public record ProductDto(int ProductId, string ProductName, decimal Price);

    public record CategoryWithProductsDto(int CategoryId, string CategoryName, IReadOnlyList<ProductDto> Products);
}
namespace ECommerceApi.JJHH17.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;
        public List<Sale> Sales { get; } = [];
    }

    public record CreateProductDto(string name, decimal price, int categoryId);

    public record GetProductsDto(int productId, string productName, decimal price, int CategoryId, string CategoryName);
}

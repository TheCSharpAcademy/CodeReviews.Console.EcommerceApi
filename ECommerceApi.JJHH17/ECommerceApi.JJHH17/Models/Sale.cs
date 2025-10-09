namespace ECommerceApi.JJHH17.Models
{
    public class Sale
    {
        public int SaleId { get; set; }
        public List<Product> Products { get; } = [];
        public decimal SalePrice => Products.Sum(p => p.Price);
        public int ItemCount => Products.Count();
    }

    public record CreateSaleDto(List<int> ProductIds);

    public record SaleWithProductsDto(int SaleId, int ItemCount, decimal SalePrice, List<ProductDto> Products);
}

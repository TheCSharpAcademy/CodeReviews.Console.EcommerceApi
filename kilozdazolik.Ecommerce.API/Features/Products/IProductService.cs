namespace kilozdazolik.Ecommerce.API.Features.Products
{
    public interface IProductService
    {
        Task<ProductDto> CreateProductAsync(CreateProductDto product);
        Task<ProductDto?> GetProductByIdAsync(Guid id);
        Task<IEnumerable<ProductDto>> GetProductsAsync(ProductParameters parameters);
        Task UpdateProductAsync(Guid id, UpdateProductDto dto);
        Task DeleteProductAsync(Guid id);
        Task RestoreProductAsync(Guid id);
    }
}

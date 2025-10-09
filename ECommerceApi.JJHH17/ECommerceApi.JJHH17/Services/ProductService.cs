using ECommerceApi.JJHH17.Data;
using ECommerceApi.JJHH17.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ECommerceApi.JJHH17.Services
{
    public interface IProductService
    {
        public List<GetProductsDto> GetAllProducts();
        public GetProductsDto? GetProductById(int id);
        public GetProductsDto CreateProduct(CreateProductDto product);
    }

    public class ProductService : IProductService
    {
        private readonly ProductsDbContext _dbContext;

        public ProductService(ProductsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public GetProductsDto CreateProduct(CreateProductDto product)
        {
            if (product == null)
                throw new ArgumentNullException("Product name is required.", nameof(product));

            var categoryExists = _dbContext.Categories
                .Any(c => c.CategoryId == product.categoryId);
            if (!categoryExists)
                throw new ArgumentException("Category does not exist", nameof(product.categoryId));

            var newProduct = new Product { ProductName = product.name.Trim(), Price = product.price, CategoryId = product.categoryId };

            _dbContext.Products.Add(newProduct);
            _dbContext.SaveChanges();

            return _dbContext.Products
                .AsNoTracking()
                .Where(p => p.ProductId == newProduct.ProductId)
                .Select(p => new GetProductsDto(
                    p.ProductId,
                    p.ProductName,
                    p.Price,
                    p.CategoryId,
                    p.Category.CategoryName))
                .Single();
        }

        public List<GetProductsDto> GetAllProducts()
        {
            return _dbContext.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .Select(p => new GetProductsDto(
                    p.ProductId,
                    p.ProductName,
                    p.Price,
                    p.CategoryId,
                    p.Category.CategoryName))
                .ToList();
        }

        public GetProductsDto GetProductById(int id)
        {
            var productItem = _dbContext.Products.Find(id);

            if (productItem == null)
            {
                return null;
            }

            return _dbContext.Products
                .AsNoTracking()
                .Where(p => p.ProductId == id)
                .Select(p => new GetProductsDto(
                    p.ProductId, p.ProductName, p.Price, p.CategoryId, p.Category.CategoryName))
                .SingleOrDefault();
        }
    }
}

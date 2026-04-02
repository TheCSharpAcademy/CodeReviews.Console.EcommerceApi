using ECommerce.Api.TerrenceLGee.Data;
using ECommerce.Api.TerrenceLGee.Repositories.Helpers;
using ECommerce.Contracts.TerrenceLGee.Common.Extensions;
using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Api.TerrenceLGee.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ECommerceDbContext _context;
    private readonly ILogger<ProductRepository> _logger;
    private string _errorMessage = string.Empty;

    public ProductRepository(ECommerceDbContext context, ILogger<ProductRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Product?> AddProductAsync(Product product)
    {
        try
        {
            var productAlreadyExists = await _context.Products
                .AnyAsync(p => p.Name.ToLower().Equals(product.Name.ToLower()));

            if (productAlreadyExists) return null;

            product.CreatedAt = DateTime.UtcNow;

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == product.Id);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(AddProductAsync)}\n" +
                $"There was an unexpected error adding a new product to category {product.CategoryId}: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }

    }

    public async Task<Product?> UpdateProductAsync(Product product)
    {
        try
        {
            var productToUpdate = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (productToUpdate is null) return null;

            productToUpdate.Name = product.Name;
            productToUpdate.Description = product.Description;
            productToUpdate.StockQuantity = product.StockQuantity;
            productToUpdate.DiscountPercentage = product.DiscountPercentage;
            productToUpdate.ImageUrl = product.ImageUrl;

            if (productToUpdate.StockQuantity <= 0)
            {
                productToUpdate.IsInStock = false;
            }
            else
            {
                productToUpdate.IsInStock = true;
            }

            productToUpdate.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return productToUpdate;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(UpdateProductAsync)}\n" +
                $"There was an unexpected error updating product {product.Id} in category {product.CategoryId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<bool> DeleteProductAsync(int productId)
    {
        try
        {
            var productToDelete = await _context.Products
                .FirstOrDefaultAsync(p => p.Id == productId);

            if (productToDelete is null || productToDelete.IsDeleted) return false;

            productToDelete.IsDeleted = true;
            productToDelete.IsInStock = false;
            productToDelete.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(DeleteProductAsync)}\n" +
                $"There was an unexpected error 'deleting' product: {productId}:" +
                $"{ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
    }

    public async Task<bool> RestoreProductAsync(int productId)
    {
        try
        {
            var productToRestore = await _context.Products
                 .FirstOrDefaultAsync(p => p.Id == productId && p.IsDeleted);

            if (productToRestore is null || !productToRestore.IsDeleted) return false;

            productToRestore.IsDeleted = false;
            if (productToRestore.StockQuantity > 0) productToRestore.IsInStock = true;
            productToRestore.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
            $"Method: {nameof(RestoreProductAsync)}\n" +
            $"There was an unexpected error 'restoring' product {productId}: " +
            $"{ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return false;
        }
    }

    public async Task<Product?> GetProductAsync(int productId)
    {
        try
        {
            var product = await _context.Products
                .Include(p => p.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == productId);

            return product;
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(GetProductAsync)}\n" +
                $"There was an unexpected error retrieving product {productId}: " +
                $"{ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return null;
        }
    }

    public async Task<PagedList<Product>> GetProductsAsync(ProductQueryParams productQueryParams)
    {
        try
        {
            var products = _context.Products
                .Include(p => p.Category)
                .AsNoTracking();

            SetFilteringAndSorting(ref products, productQueryParams);

            return await products.ToPagedListAsync(products.Count(), productQueryParams.Page, productQueryParams.PageSize);
        }
        catch (Exception ex)
        {
            _errorMessage = $"\nClass: {nameof(ProductRepository)}\n" +
                $"Method: {nameof(GetProductsAsync)}\n" +
                $"There was an unexpected error retrieving all products: {ex.Message}";
            _logger.LogError(ex, "{msg}\n\n", _errorMessage);
            return [];
        }
    }

    private void SetFilteringAndSorting(ref IQueryable<Product> products, ProductQueryParams productQueryParams)
    {
        if (!string.IsNullOrEmpty(productQueryParams.CategoryName))
        {
            products = products.Where(p => p.Category != null &&
            p.Category.Name.ToLower().Equals(productQueryParams.CategoryName.ToLower()));
        }

        if (!string.IsNullOrEmpty(productQueryParams.Description))
        {
            products = products.Where(p => !string.IsNullOrEmpty(p.Description) &&
            p.Description.ToLower().Contains(productQueryParams.Description.ToLower()));
        }

        if (productQueryParams.MinUnitPrice.HasValue && productQueryParams.MaxUnitPrice.HasValue)
        {
            if (productQueryParams.IsValidUnitPriceRange)
            {
                products = products.Where(p => p.UnitPrice >= productQueryParams.MinUnitPrice &&
                p.UnitPrice <= productQueryParams.MaxUnitPrice);
            }
        }
        else if (productQueryParams.MinUnitPrice.HasValue && !productQueryParams.MaxUnitPrice.HasValue)
        {
            products = products.Where(p => p.UnitPrice >= productQueryParams.MinUnitPrice);
        }
        else if (productQueryParams.MaxUnitPrice.HasValue && !productQueryParams.MinUnitPrice.HasValue)
        {
            products = products.Where(p => p.UnitPrice <= productQueryParams.MaxUnitPrice);
        }

        if (productQueryParams.MinStockQuantity.HasValue && productQueryParams.MaxStockQuantity.HasValue)
        {
            if (productQueryParams.IsValidStockQuantityRange)
            {
                products = products.Where(p => p.StockQuantity >= productQueryParams.MinStockQuantity &&
                p.StockQuantity <= productQueryParams.MaxStockQuantity);
            }
        }
        else if (productQueryParams.MinStockQuantity.HasValue && !productQueryParams.MaxStockQuantity.HasValue)
        {
            products = products.Where(p => p.StockQuantity >= productQueryParams.MinStockQuantity);
        }
        else if (productQueryParams.MaxStockQuantity.HasValue && !productQueryParams.MinStockQuantity.HasValue)
        {
            products = products.Where(p => p.StockQuantity <= productQueryParams.MaxStockQuantity);
        }

        if (productQueryParams.MinDiscountPercentage.HasValue && productQueryParams.MaxDiscountPercentage.HasValue)
        {
            if (productQueryParams.IsValidDiscountPercentageRange)
            {
                products = products.Where(p => p.DiscountPercentage >= productQueryParams.MinDiscountPercentage &&
                p.DiscountPercentage <= productQueryParams.MaxDiscountPercentage);
            }
        }
        else if (productQueryParams.MinDiscountPercentage.HasValue && !productQueryParams.MaxDiscountPercentage.HasValue)
        {
            products = products.Where(p => p.DiscountPercentage >= productQueryParams.MinDiscountPercentage);
        }
        else if (productQueryParams.MaxDiscountPercentage.HasValue && !productQueryParams.MinDiscountPercentage.HasValue)
        {
            products = products.Where(p => p.DiscountPercentage <= productQueryParams.MaxDiscountPercentage);
        }

        if (productQueryParams.InStock.HasValue)
        {
            products = products.Where(p => p.IsInStock == productQueryParams.InStock);
        }

        if (productQueryParams.IsDeleted.HasValue)
        {
            products = products.Where(p => p.IsDeleted == productQueryParams.IsDeleted);
        }

        if (productQueryParams.CategoryId.HasValue)
        {
            products = products.Where(p => p.CategoryId == productQueryParams.CategoryId);
        }

        products = SortHelper<Product>.ApplySorting(products, productQueryParams.OrderBy);
    }


}

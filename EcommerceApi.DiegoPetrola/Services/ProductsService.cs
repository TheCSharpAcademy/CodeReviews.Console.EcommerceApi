using EcommerceApi.Context;
using EcommerceApi.Models;
using EcommerceApi.Models.DTOs;
using EcommerceApi.Results;
using EcommerceApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Services;

public class ProductsService(EcommerceDbContext context)
{
    public async Task<Result<List<ProductDto>>> GetProducts(int pageNumber, int pageSize)
    {
        var products = await context.Products
            .Include(p => p.Category)
            .Skip(pageSize * pageNumber)
            .Take(pageSize)
            .Select(p => p.ToDto())
            .ToListAsync();

        return Result<List<ProductDto>>.Ok(products);
    }

    public async Task<Result<ProductDto>> GetProduct(int id)
    {
        var product = await context.Products
            .Include(p => p.Category)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product is null)
            return Result<ProductDto>.NotFound("Product not found");

        return Result<ProductDto>.Ok(product.ToDto());
    }

    public async Task<Result<ProductDto>> CreateProduct(CreateProductDto dto)
    {
        var product = new Product
        {
            Name = dto.Name,
            Price = dto.Price,
            CategoryId = dto.CategoryId
        };

        try
        {
            context.Products.Add(product);
            await context.SaveChangesAsync();
            await context.Entry(product)
            .Reference(p => p.Category)
            .LoadAsync();
        }
        catch
        {
            return Result<ProductDto>.Invalid("Invalid product data or duplicate entry");
        }
        return Result<ProductDto>.Ok(product.ToDto());
    }

    public async Task<Result<ProductDto>> UpdateProduct(ProductDto dto)
    {
        var product = await context.Products.FindAsync(dto.Id);

        if (product is null)
            return Result<ProductDto>.NotFound("Product not found");

        try
        {
            product.CategoryId = dto.CategoryId;
            product.Name = dto.Name;

            await context.SaveChangesAsync();
            await context.Entry(product).Reference(p => p.Category).LoadAsync();
        }
        catch
        {
            return Result<ProductDto>.Invalid("Something went wrong");
        }
        return Result<ProductDto>.Ok(product.ToDto());
    }

    public async Task<Result<ProductDto?>> SoftDeleteProduct(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product is null)
            return Result<ProductDto?>.NotFound("Product not found");

        try
        {
            product.IsDeleted = true;
            await context.SaveChangesAsync();
            return Result<ProductDto?>.Ok(null);
        }
        catch
        {
            return Result<ProductDto?>.InternalServerError("Something went wrong");
        }
    }
}

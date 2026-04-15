using Ecommerce.Core.DTOs.Product;
using Ecommerce.Core.Interfaces.Repositories;
using Ecommerce.Core.Interfaces.Services;
using Ecommerce.Core.Models;
using Ecommerce.Core.Utilities;

namespace Ecommerce.Services;

public class ProductService(IUnitOfWork unitOfWork) : IProductService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<ProductDto>> CreateProductAsync(CreateProductDto product)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);
        if (category is null)
        {
            return Result<ProductDto>.Fail($"Category with Id {product.CategoryId} was not found");
        }
        
        Product newProduct = new()
        {
            Name = product.Name,
            Description = product.Description,
            ImageUrl = product.ImageUrl,
            Quantity = product.Quantity,
            Price = product.Price,
            CategoryId = product.CategoryId
        };
        
        _unitOfWork.Products.Add(newProduct);
        int rowsAffected = await _unitOfWork.CompleteAsync();
        if (rowsAffected == 0)
        {
            return Result<ProductDto>.Fail("Failed to create product");
        }
        ProductDto dto = new()
        {
            Id = newProduct.Id,
            Name = newProduct.Name,
            ImageUrl = newProduct.ImageUrl,
            Quantity = newProduct.Quantity,
            Description = newProduct.Description,
            Price = newProduct.Price,
            CategoryId = newProduct.CategoryId,
            CategoryName = category.Name
        };
        return Result<ProductDto>.Success(dto);
    }

    public async Task<Result<ProductDto>> GetProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id, p => p.Category);
        if (product is null)
        {
            return Result<ProductDto>.Fail($"Product with Id {id} was not found");
        }

        ProductDto dto = new()
        {
            Id = product.Id,
            Name = product.Name,
            ImageUrl = product.ImageUrl,
            Quantity = product.Quantity,
            Description = product.Description,
            Price = product.Price,
            CategoryId = product.CategoryId,
            CategoryName = product.Category?.Name ?? string.Empty
        };
        return Result<ProductDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetAllProductsAsync(PaginationParams paginationParams)
    {
        var products = await _unitOfWork.Products.GetAllAsync(paginationParams, p => p.Category);
        return Result<IEnumerable<ProductDto>>.Success(products.Select(p => new ProductDto()
        {
            Id = p.Id,
            Name = p.Name,
            ImageUrl = p.ImageUrl,
            Quantity = p.Quantity,
            Description = p.Description,
            Price = p.Price,
            CategoryId = p.CategoryId,
            CategoryName = p.Category?.Name ?? string.Empty
        }));
    }

    public async Task<Result<ProductDto>> UpdateProductAsync(int id, UpdateProductDto product)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(product.CategoryId);
        if (category is null)
            return Result<ProductDto>.Fail($"Category with Id {id} was not found");
        
        var productFromDb = await _unitOfWork.Products.GetByIdAsync(id);
        if(productFromDb is null)
            return Result<ProductDto>.Fail($"Product with Id {id} was not found");
        
        productFromDb.Name = product.Name;
        productFromDb.Description = product.Description;
        productFromDb.Price = product.Price;
        productFromDb.Quantity = product.Quantity;
        productFromDb.ImageUrl = product.ImageUrl;
        productFromDb.CategoryId = product.CategoryId;
        
        await _unitOfWork.CompleteAsync();
        
        return Result<ProductDto>.Success(new ProductDto()
        {
            Id = productFromDb.Id,
            Name = productFromDb.Name,
            ImageUrl = productFromDb.ImageUrl,
            Quantity = productFromDb.Quantity,
            Description = productFromDb.Description,
            Price = productFromDb.Price,
            CategoryId = productFromDb.CategoryId,
            CategoryName = category.Name
        });
    }

    public async Task<Result<bool>> DeleteProductAsync(int id)
    {
        var product = await _unitOfWork.Products.GetByIdAsync(id);
        if(product is null)
            return Result<bool>.Fail($"Product with Id {id} was not found");
        _unitOfWork.Products.Delete(product);
        var rowsAffected = await _unitOfWork.CompleteAsync();
        if (rowsAffected == 0)
        {
            return Result<bool>.Fail("Failed to delete product");
        }
        return Result<bool>.Success(true);
    }
}
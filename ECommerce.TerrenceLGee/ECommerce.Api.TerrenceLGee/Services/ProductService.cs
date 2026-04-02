using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Contracts.TerrenceLGee.Mappings.ProductMappings;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.ProductParameters;

namespace ECommerce.Api.TerrenceLGee.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<RetrievedProductForAdminDto?>> AddProductAsync(CreateProductDto product)
    {
        var addedProduct = await _productRepository.AddProductAsync(product.FromCreateProductDto());

        if (addedProduct is null)
        {
            return Result<RetrievedProductForAdminDto?>.Fail("Unable to add new product at this time.", ErrorType.BadRequest);
        }

        return Result<RetrievedProductForAdminDto?>.Ok(addedProduct.ToRetrievedProductForAdminDto());
    }

    public async Task<Result<RetrievedProductForAdminDto?>> UpdateProductAsync(UpdateProductDto product)
    {
        var updatedProduct = await _productRepository.UpdateProductAsync(product.FromUpdateProductDto());

        if (updatedProduct is null)
        {
            return Result<RetrievedProductForAdminDto?>.Fail($"Unable to update product {product.Id}.", ErrorType.BadRequest);
        }

        return Result<RetrievedProductForAdminDto?>.Ok(updatedProduct.ToRetrievedProductForAdminDto());
    }

    public async Task<Result> DeleteProductAsync(ProductParams productParams)
    {
        var deleted = await _productRepository.DeleteProductAsync(productParams.ProductId);

        if (!deleted)
        {
            return Result.Fail($"Deletion of product {productParams.ProductId} failed.", ErrorType.NotFound);
        }

        return Result.Ok();
    }

    public async Task<Result> RestoreProductAsync(ProductParams productParams)
    {
        var restored = await _productRepository.RestoreProductAsync(productParams.ProductId);

        if (!restored)
        {
            return Result.Fail($"Restoration of product {productParams.ProductId} failed", ErrorType.NotFound);
        }

        return Result.Ok();
    }

    public async Task<Result<RetrievedProductDto?>> GetProductAsync(ProductParams productParams)
    {
        var product = await _productRepository.GetProductAsync(productParams.ProductId);

        if (product is null)
        {
            return Result<RetrievedProductDto?>.Fail($"Unable to retrieve product {productParams.ProductId}", ErrorType.NotFound);
        }

        return Result<RetrievedProductDto?>.Ok(product.ToRetrievedProductDto());
    }

    public async Task<Result<RetrievedProductForAdminDto?>> GetProductForAdminAsync(ProductParams productParams)
    {
        var product = await _productRepository.GetProductAsync(productParams.ProductId);

        if (product is null)
        {
            return Result<RetrievedProductForAdminDto?>.Fail($"Unable to retrieve product {productParams.ProductId}", ErrorType.NotFound);
        }

        return Result<RetrievedProductForAdminDto?>.Ok(product.ToRetrievedProductForAdminDto());
    }

    public async Task<Result<PagedList<RetrievedProductDto>>> GetProductsAsync(ProductQueryParams productQueryParams)
    {
        var products = await _productRepository.GetProductsAsync(productQueryParams);

        return Result<PagedList<RetrievedProductDto>>.Ok(new PagedList<RetrievedProductDto>(
            products.Select(p => p.ToRetrievedProductDto()),
            products.TotalEntities,
            productQueryParams.Page,
            productQueryParams.PageSize));
    }


    public async Task<Result<PagedList<RetrievedProductForAdminDto>>> GetProductsForAdminAsync(ProductQueryParams productQueryParams)
    {
        var products = await _productRepository.GetProductsAsync(productQueryParams);

        return Result<PagedList<RetrievedProductForAdminDto>>.Ok(new PagedList<RetrievedProductForAdminDto>(
            products.Select(p => p.ToRetrievedProductForAdminDto()),
            products.TotalEntities,
            productQueryParams.Page,
            productQueryParams.PageSize));
    }
}

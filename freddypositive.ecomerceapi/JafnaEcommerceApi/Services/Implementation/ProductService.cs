using AutoMapper;
using JafnaEcommerceApi.Exceptions;
using JafnaEcommerceApi.Models.DTOs.PaginationDTOs;
using JafnaEcommerceApi.Models.DTOs.ProductDTOs;
using JafnaEcommerceApi.Models.Entities;
using JafnaEcommerceApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JafnaEcommerceApi.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductService(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }
    public async Task CreateAsync(ProductCreateDto productCreateDto)
    {
        var existingProductName = await _productRepository.GetNameAsync(productCreateDto.Name);

        if (existingProductName != null)
            throw new ConflictException("Product already exists.");

        var productEntity = _mapper.Map<Product>(productCreateDto);

        await _productRepository.CreateAsync(productEntity);
    }

    public async Task UpdateAsync(int productId, ProductUpdateDto productUpdateDto)
    {
        var existingProductDetails = await _productRepository.GetByIdAsync(productId);

        if (existingProductDetails == null)
            throw new ConflictException("Product not found");

        _mapper.Map(productUpdateDto, existingProductDetails);

        await _productRepository.UpdateAsync(existingProductDetails);
    }

    public async Task DeleteAsync(int productId)
    {
        var existingProductDetails = await _productRepository.GetByIdAsync(productId);

        if (existingProductDetails == null)
            throw new ConflictException("Product not found");

        existingProductDetails.IsDeleted = true;

        await _productRepository.DeleteAsync(existingProductDetails);
    }
    public async Task<PagedResponse<ProductDto>> GetAllAsync(int pageNumber, int pageSize)
    {
        var productQuery = _productRepository.GetProductQuery();

        var productCount = await productQuery.CountAsync();

        var productList = await productQuery
                      .Skip((pageNumber - 1) * pageSize)
                      .Take(pageSize)
                      .ToListAsync();

        var productDto = _mapper.Map<List<ProductDto>>(productList);

        return new PagedResponse<ProductDto>(productDto, productCount, pageNumber, pageSize);
    }

    public async Task<PagedResponse<ProductDto>> GetProductsByCategoryAsync(int categoryId, int pageNumber, int pageSize)
    {
        var categoryProductQuery = _productRepository.GetCategoryProductQuery(categoryId);

        var productCount = await categoryProductQuery.CountAsync();

        var productList = await categoryProductQuery
                      .Skip((pageNumber - 1) * pageSize)
                      .Take(pageSize)
                      .ToListAsync();

        var productDto = _mapper.Map<List<ProductDto>>(productList);

        return new PagedResponse<ProductDto>(productDto, productCount, pageNumber, pageSize);

    }
}

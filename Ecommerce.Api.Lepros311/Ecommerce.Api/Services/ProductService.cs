using Ecommerce.Api.Models;
using Ecommerce.Api.Repository;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;

    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResponse<List<ProductDto>>> GetPagedProducts(PaginationParams paginationParams)
    {
        var response = new PagedResponse<List<Product>>(data: new List<Product>(),
                                               pageNumber: paginationParams.Page,
                                               pageSize: paginationParams.PageSize,
                                               totalRecords: 0);
        var responseWithDataDto = new PagedResponse<List<ProductDto>>(data: new List<ProductDto>(),
                                               pageNumber: paginationParams.Page,
                                               pageSize: paginationParams.PageSize,
                                               totalRecords: 0);

        response  = await _productRepository.GetPagedProducts(paginationParams);

        if (response.Status == ResponseStatus.Fail)
        {
            responseWithDataDto.Status = response.Status;
            responseWithDataDto.Message = response.Message;
            return responseWithDataDto;
        }

        responseWithDataDto.Data = response.Data.Select(p => new ProductDto
        {
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Price = p.Price,
            Category = p.Category.CategoryName
        }).ToList();

        return responseWithDataDto;
    }

    public async Task<BaseResponse<Product>> GetProductById(int id)
    {
        return await _productRepository.GetProductById(id);
    }

    public async Task<BaseResponse<ProductDto>> CreateProduct(WriteProductDto writeProductDto)
    {
        var productResponse = new BaseResponse<Product>();
        var productResponseWithDataDto = new BaseResponse<ProductDto>();

        var categoryResponse = await _categoryRepository.GetCategoryById(writeProductDto.CategoryId);

        if (categoryResponse.Status == ResponseStatus.Fail)
        {
            productResponseWithDataDto.Status = ResponseStatus.Fail;
            productResponseWithDataDto.Message = categoryResponse.Message;
            return productResponseWithDataDto;
        }

        if (writeProductDto.Price < 0)
        {
            productResponseWithDataDto.Status = ResponseStatus.Fail;
            productResponseWithDataDto.Message = "Product price must be greater than 0.";
            return productResponseWithDataDto;
        }

        var newProduct = new Product
        {
            ProductName = writeProductDto.ProductName,
            Price = writeProductDto.Price,
            CategoryId = writeProductDto.CategoryId
        };

        newProduct.Category = categoryResponse.Data;

        productResponse = await _productRepository.CreateProduct(newProduct);

        if (productResponse.Status == ResponseStatus.Fail)
        {
            productResponseWithDataDto.Status = ResponseStatus.Fail;
            productResponseWithDataDto.Message = productResponse.Message;
            return productResponseWithDataDto;
        }
        else
        {
            productResponseWithDataDto.Status = ResponseStatus.Success;

            var newProductDto = new ProductDto
            {
                ProductId = newProduct.ProductId,
                ProductName = newProduct.ProductName,
                Price = newProduct.Price,
                Category = newProduct.Category?.CategoryName
            };

            productResponseWithDataDto.Data = newProductDto;
        }

        return productResponseWithDataDto;
    }

    public async Task<BaseResponse<Product>> UpdateProduct(int id, WriteProductDto writeProductDto)
    {
        var productResponse = new BaseResponse<Product>();

        productResponse = await GetProductById(id);

        if (productResponse.Status == ResponseStatus.Fail)
        {
            return productResponse;
        }

        if (writeProductDto.Price < 0)
        {
            productResponse.Status = ResponseStatus.Fail;
            productResponse.Message = "Product price must be greater than 0.";
            return productResponse;
        }

        var existingProduct = productResponse.Data;

        existingProduct.ProductName = writeProductDto.ProductName;
        existingProduct.Price = writeProductDto.Price;

        var categoryResponse = await _categoryRepository.GetCategoryById(writeProductDto.CategoryId);

        if (categoryResponse.Status == ResponseStatus.Fail)
        {
            productResponse.Status = ResponseStatus.Fail;
            productResponse.Message = categoryResponse.Message;
            return productResponse;
        }

        existingProduct.CategoryId = writeProductDto.CategoryId;

        productResponse = await _productRepository.UpdateProduct(existingProduct);

        return productResponse;
    }

    public async Task<BaseResponse<Product>> DeleteProduct(int id)
    {
        var response = new BaseResponse<Product>();

        response = await _productRepository.GetProductById(id);

        if (response.Status == ResponseStatus.Fail)
        {
            return response;
        }

        return await _productRepository.DeleteProduct(id);
    }
}

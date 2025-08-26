using Ecommerce.Api.Models;
using Ecommerce.Api.Repository;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public class CategoryService : ICategoryService
{
    private readonly IProductRepository _productRepository;

    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<PagedResponse<List<CategoryDto>>> GetPagedCategories(PaginationParams paginationParams)
    {
        var response = new PagedResponse<List<Category>>(data: new List<Category>(),
                                               pageNumber: paginationParams.Page,
                                               pageSize: paginationParams.PageSize,
                                               totalRecords: 0);
        var responseWithDataDto = new PagedResponse<List<CategoryDto>>(data: new List<CategoryDto>(),
                                               pageNumber: paginationParams.Page,
                                               pageSize: paginationParams.PageSize,
                                               totalRecords: 0);

        response = await _categoryRepository.GetPagedCategories(paginationParams);

        if (response.Status == ResponseStatus.Fail)
        {
            responseWithDataDto.Status = response.Status;
            responseWithDataDto.Message = response.Message;
            return responseWithDataDto;
        }

        responseWithDataDto.Data = response.Data.Select(c => new CategoryDto
        {
            CategoryId = c.CategoryId,
            CategoryName = c.CategoryName,
            Products = c.Products.Select(p => new ProductDto
            {
                ProductId = p.ProductId,
                ProductName = p.ProductName,
                Price = p.Price,
                Category = p.Category.CategoryName
            }).ToList()
        }).ToList();

        return responseWithDataDto;
    }

    public async Task<BaseResponse<Category>> GetCategoryById(int id)
    {
        return await _categoryRepository.GetCategoryById(id);
    }

    public async Task<BaseResponse<CategoryDto>> CreateCategory(WriteCategoryDto writeCategoryDto)
    {
        var response = new BaseResponse<Category>();
        var responseWithDataDto = new BaseResponse<CategoryDto>();

        var newCategory = new Category
        {
            CategoryName = writeCategoryDto.CategoryName
        };

        response = await _categoryRepository.CreateCategory(newCategory);

        if (response.Status == ResponseStatus.Fail)
        {
            responseWithDataDto.Status = ResponseStatus.Fail;
            responseWithDataDto.Message = response.Message;
            return responseWithDataDto;
        }
        else
        {
            responseWithDataDto.Status = ResponseStatus.Success;

            var newCategoryDto = new CategoryDto
            {
                CategoryId = newCategory.CategoryId,
                CategoryName = newCategory.CategoryName,
            };

            responseWithDataDto.Data = newCategoryDto;
        }

        return responseWithDataDto;
    }

    public async Task<BaseResponse<Category>> UpdateCategory(int id, WriteCategoryDto writeCategoryDto)
    {
        var response = new BaseResponse<Category>();

        response = await GetCategoryById(id);

        if (response.Status == ResponseStatus.Fail)
        {
            return response;
        }

        var existingCategory = response.Data;

        existingCategory.CategoryName = writeCategoryDto.CategoryName;

        response = await _categoryRepository.UpdateCategory(existingCategory);

        return response;
    }

    public async Task<BaseResponse<Category>> DeleteCategory(int id)
    {
        var response = new BaseResponse<Category>();

        response = await _categoryRepository.GetCategoryById(id);

        if (response.Status == ResponseStatus.Fail)
        {
            return response;
        }

        if (response.Data.Products.Count > 0)
        {
            response.Message = "Cannot delete categories that contain products.";
            return response;
        }

        return await _categoryRepository.DeleteCategory(id);
    }
}


using Ecommerce.Core.DTOs.Category;
using Ecommerce.Core.Interfaces.Repositories;
using Ecommerce.Core.Interfaces.Services;
using Ecommerce.Core.Models;
using Ecommerce.Core.Utilities;

namespace Ecommerce.Services;

public class CategoryService(IUnitOfWork unitOfWork) : ICategoryService
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Result<CategoryDto>> CreateCategoryAsync(CreateCategoryDto category)
    {
        Category newCategory = new()
        {
            Name = category.Name,
            Description = category.Description
        };
        _unitOfWork.Categories.Add(newCategory);
        int rowsAffected = await _unitOfWork.CompleteAsync();
        if (rowsAffected == 0)
        {
            return Result<CategoryDto>.Fail("Failed to create category");
        }

        CategoryDto dto = new()
        {
            Id = newCategory.Id,
            Name = newCategory.Name,
            Description = newCategory.Description
        };
        return Result<CategoryDto>.Success(dto);
    }

    public async Task<Result<CategoryDto>> GetCategoryAsync(int id)
    {
        var category = await _unitOfWork.Categories.GetByIdAsync(id);
        if (category is null)
        {
            return Result<CategoryDto>.Fail($"Category with Id {id} was not found");
        }

        CategoryDto dto = new()
        {
            Id = category.Id,
            Name = category.Name,
            Description = category.Description
        };
        return Result<CategoryDto>.Success(dto);
    }

    public async Task<Result<IEnumerable<CategoryDto>>> GetAllCategoriesAsync()
    {
        var categories = await _unitOfWork.Categories.GetAllAsync();
        return Result<IEnumerable<CategoryDto>>.Success(
            categories.Select(c => new CategoryDto()
            {
                Id= c.Id,
                Name = c.Name,
                Description = c.Description
            })
        );
    }

    public async Task<Result<CategoryDto>> UpdateCategoryAsync(int id, UpdateCategoryDto category)
    {
        var categoryFromDb = await _unitOfWork.Categories.GetByIdAsync(id);
        if(categoryFromDb is null)
            return Result<CategoryDto>.Fail($"Category with Id {id} was not found");
        categoryFromDb.Name = category.Name;
        categoryFromDb.Description = category.Description;

        await _unitOfWork.CompleteAsync();
        
        return Result<CategoryDto>.Success(new CategoryDto()
        {
            Id = categoryFromDb.Id,
            Name = categoryFromDb.Name,
            Description = categoryFromDb.Description
        });
    }

    public async Task<Result<bool>> DeleteCategoryAsync(int id)
    {
        var categoryFromDb = await _unitOfWork.Categories.GetByIdAsync(id);
        if(categoryFromDb is null)
            return Result<bool>.Fail($"Category with Id {id} was not found");
        
        _unitOfWork.Categories.Delete(categoryFromDb);
        var rowsAffected = await _unitOfWork.CompleteAsync();
        if (rowsAffected == 0)
        {
            return Result<bool>.Fail("Failed to delete category");
        }
        return Result<bool>.Success(true);
    }
}
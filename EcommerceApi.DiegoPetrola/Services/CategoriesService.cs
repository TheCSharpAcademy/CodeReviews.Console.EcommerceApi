using EcommerceApi.Context;
using EcommerceApi.Models;
using EcommerceApi.Models.DTOs;
using EcommerceApi.Results;
using EcommerceApi.Utils;
using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Services;

public class CategoriesService(EcommerceDbContext context)
{
    public async Task<Result<List<CategoryDto>>> GetCategories()
    {
        var categories = await context.Categories
            .Select(c => c.ToDto())
            .ToListAsync();

        return Result<List<CategoryDto>>.Ok(categories);
    }

    public async Task<Result<CategoryDto>> GetCategory(int id)
    {
        var category = await context.Categories.FindAsync(id);
        if (category is null)
            return Result<CategoryDto>.NotFound("Category not found");
        return Result<CategoryDto>.Ok(category.ToDto());
    }

    public async Task<Result<CategoryDto?>> DeleteCategory(int id)
    {
        var category = await context.Categories.FindAsync(id);
        var products = await context.Products.AnyAsync(p => p.CategoryId == id);
        if (products)
            return Result<CategoryDto?>.Invalid("Can't delete a category that has products");
        if (category is null)
            return Result<CategoryDto?>.NotFound("Category not found");
        try
        {
            context.Remove(category);
            await context.SaveChangesAsync();
            return Result<CategoryDto?>.Ok(null);
        }
        catch
        {
            return Result<CategoryDto?>.InternalServerError("Problem on the server");
        }
    }

    public async Task<Result<CategoryDto>> CreateCategory(CreateCategoryDto dto)
    {
        var category = new Category { Name = dto.Name };
        try
        {
            context.Categories.Add(category);
            await context.SaveChangesAsync();
        }
        catch
        {
            return Result<CategoryDto>.Invalid("Duplicated category");
        }

        return Result<CategoryDto>.Ok(category.ToDto());
    }

    public async Task<Result<CategoryDto>> UpdateCategory(CategoryDto dto)
    {
        var category = await context.Categories.FindAsync(dto.Id);
        if (category is null)
            return Result<CategoryDto>.NotFound("Category not found");
        category.Name = dto.Name;
        try
        {
            await context.SaveChangesAsync();
            return Result<CategoryDto>.Ok(category.ToDto());
        }
        catch
        {
            return Result<CategoryDto>.Invalid("Duplicated category");
        }
    }
}

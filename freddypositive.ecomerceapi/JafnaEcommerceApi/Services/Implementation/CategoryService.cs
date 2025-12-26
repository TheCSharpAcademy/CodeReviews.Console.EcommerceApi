using AutoMapper;
using JafnaEcommerceApi.Exceptions;
using JafnaEcommerceApi.Models.DTOs.CategoryDTOs;
using JafnaEcommerceApi.Models.DTOs.PaginationDTOs;
using JafnaEcommerceApi.Models.Entities;
using JafnaEcommerceApi.Repositories;
using Microsoft.EntityFrameworkCore;

namespace JafnaEcommerceApi.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CategoryService(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task Create(CategoryCreateDto categoryCreateDto)
    {
        var existing = await _categoryRepository.GetByNameAsync(categoryCreateDto.Name);
        if (existing != null)
            throw new ConflictException("Category already exists.");

        var categoryEntity = _mapper.Map<Category>(categoryCreateDto);
        await _categoryRepository.Create(categoryEntity);
    }

    public async Task Update(int categoryId, CategoryUpdateDto categoryUpdateDto)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(categoryId);
        if (existingCategory == null)
            throw new ConflictException("Category not found");

        if (!string.IsNullOrWhiteSpace(categoryUpdateDto.Name))
        {
            var name = await _categoryRepository.GetByNameAsync(categoryUpdateDto.Name);
            if (name != null && name.Id != categoryId)
                throw new ConflictException("Category already exists.");
        }

        _mapper.Map(categoryUpdateDto, existingCategory);
        await _categoryRepository.Update(existingCategory);
    }
    public async Task Delete(int categoryId)
    {
        var existingCategory = await _categoryRepository.GetByIdAsync(categoryId);
        if (existingCategory == null)
            throw new ConflictException("Category not found");

        existingCategory.IsDeleted = true;
        await _categoryRepository.Delete(existingCategory);
    }

    public async Task<PagedResponse<CategoryDto>> GetAll(int pageNumber, int pageSize)
    {
        var categoryQuery = _categoryRepository.GetCategoryQuery();

        var categoryCount = await categoryQuery.CountAsync();

        var catergoryList = await categoryQuery
                      .Skip((pageNumber - 1) * pageSize)
                      .Take(pageSize)
                      .ToListAsync();

        var categoryDto = _mapper.Map<List<CategoryDto>>(catergoryList);

        return new PagedResponse<CategoryDto>(categoryDto, categoryCount, pageNumber, pageSize);
    }
}

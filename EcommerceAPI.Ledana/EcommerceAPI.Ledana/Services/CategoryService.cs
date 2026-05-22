using AutoMapper;
using Azure;
using EcommerceAPI.Ledana.Data;
using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Interfaces;
using EcommerceAPI.Ledana.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EcommerceAPI.Ledana.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ProductContext _dbContext;
        private readonly IMapper _mapper;

        public CategoryService(ProductContext productContext, IMapper mapper)
        {
            _dbContext = productContext;
            _mapper = mapper;
        }

        public async Task<ApiResponseDto<List<Category>>> GetAllCategories()
        {
            var categories = await _dbContext.Categories.Include(c => c.Products)
                .ToListAsync();

            if (categories is null)
                return new ApiResponseDto<List<Category>>
                {
                    RequestFailed = true,
                    ResponseCode = HttpStatusCode.BadRequest,
                    ErrorMessage = "Couldn't get categories",
                    Data = null
                };

            return new ApiResponseDto<List<Category>>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                Data = categories
            };
        }

        public async Task<ApiResponseDto<Category>> GetCategoryById(int id)
        {
            var category = await _dbContext.Categories.Include(c => c.Products).FirstAsync(c => c.Id == id);

            if (category is null) return new ApiResponseDto<Category>
            {
                RequestFailed = true,
                ResponseCode = HttpStatusCode.NotFound,
                Data = null,
                ErrorMessage = "Couldn't find the category"
            };

            return new ApiResponseDto<Category>
            {
                RequestFailed = false,
                ResponseCode = HttpStatusCode.OK,
                Data = category
            };
        }
        public async Task<ApiResponseDto<Category>> CreateCategory(CategoryDto category)
        {
            var newCategory = _mapper.Map<Category>(category);

            var response = await _dbContext.Categories.AddAsync(newCategory);
            await _dbContext.SaveChangesAsync();

            return new()
            {
                Data = response.Entity,
                ResponseCode = HttpStatusCode.Created
            };
        }

        public async Task<ApiResponseDto<Category>> UpdateCategory(int id, CategoryDto category)
        {
            var existingCategory = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);

            if (existingCategory is null) 
                return new()
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = "Couldn't find the category"
                };

            existingCategory = _mapper.Map(category, existingCategory);

            await _dbContext.SaveChangesAsync();

            return new()
            {
                Data = existingCategory,
                ResponseCode = HttpStatusCode.OK
            };
        }

        public async Task<ApiResponseDto<string?>> SoftDeleteCategory(int id)
        {
            var category = await _dbContext.Categories
    .Include(c => c.Products)
    .FirstOrDefaultAsync(c => c.Id == id);


            if (category is null)
            {
                return new ApiResponseDto<string?>
                {
                    RequestFailed = true,
                    Data = null,
                    ResponseCode = HttpStatusCode.NotFound,
                    ErrorMessage = $"Resource with id {id} was not found"
                };
            }

            category.IsDeleted = true;
            category.DeletedOnUtc = DateTime.UtcNow;

            foreach (var item in category.Products)
            {
                item.IsDeleted = true;
                item.DeletedOnUtc = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync();

            return new ApiResponseDto<string?>
            {
                Data = $"Category with id {id} is deleted!",
                ResponseCode = HttpStatusCode.NoContent
            };
        }

    }
}

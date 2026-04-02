using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.CategoryMappings;

public static class FromDto
{
    extension(CreateCategoryDto categoryDto)
    {
        public Category FromCreateCategoryDto()
        {
            return new Category
            {
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                CreatedAt = categoryDto.CreatedAt,
                UpdatedAt = categoryDto.UpdatedAt
            };
        }
    }

    extension(UpdateCategoryDto categoryDto)
    {
        public Category FromUpdateCategoryDto()
        {
            return new Category
            {
                Id = categoryDto.Id,
                Name = categoryDto.Name,
                Description = categoryDto.Description,
                UpdatedAt = categoryDto.UpdatedAt
            };
        }
    }
}
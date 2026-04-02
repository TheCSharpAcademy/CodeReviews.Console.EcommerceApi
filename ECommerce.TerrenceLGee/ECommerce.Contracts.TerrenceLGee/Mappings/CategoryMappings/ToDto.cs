using ECommerce.Contracts.TerrenceLGee.Mappings.ProductMappings;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;
using System.Xml.Linq;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.CategoryMappings;

public static class ToDto
{
    extension(Category category)
    {
        public RetrievedCategoryDto ToRetrievedCategoryDto()
        {
            return new RetrievedCategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };
        }

        public RetrievedCategorySummaryDto ToRetrievedCategorySummaryDto()
        {
            return new RetrievedCategorySummaryDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
            };
        }

        public RetrievedCategoryForAdminDto ToRetrievedCategoryForAdminDto()
        {
            return new RetrievedCategoryForAdminDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }

        public RetrievedCategorySummaryForAdminDto ToRetrievedCategorySummaryForAdminDto()
        {
            return new RetrievedCategorySummaryForAdminDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                CreatedAt = category.CreatedAt,
                UpdatedAt = category.UpdatedAt
            };
        }
    }
}
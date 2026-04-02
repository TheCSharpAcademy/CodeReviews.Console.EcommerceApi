using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.ProductMappings;

public static class ToDto
{
    extension(Product product)
    {
        public RetrievedProductDto ToRetrievedProductDto()
        {
            return new RetrievedProductDto
            {
                Id = product.Id,
                CategoryName = product.Category?.Name ?? "N/A",
                Name = product.Name,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                UnitPrice = product.UnitPrice,
                DiscountPercentage = product.DiscountPercentage,
                IsInStock = product.IsInStock,
                ImageUrl = product.ImageUrl
            };
        }

        public RetrievedProductForAdminDto ToRetrievedProductForAdminDto()
        {
            return new RetrievedProductForAdminDto
            {
                Id = product.Id,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "N/A",
                Name = product.Name,
                Description = product.Description,
                StockQuantity = product.StockQuantity,
                UnitPrice = product.UnitPrice,
                DiscountPercentage = product.DiscountPercentage,
                IsDeleted = product.IsDeleted,
                IsInStock = product.IsInStock,
                CreatedAt = product.CreatedAt,
                UpdatedAt = product.UpdatedAt,
                ImageUrl = product.ImageUrl
            };
        }
    }
}
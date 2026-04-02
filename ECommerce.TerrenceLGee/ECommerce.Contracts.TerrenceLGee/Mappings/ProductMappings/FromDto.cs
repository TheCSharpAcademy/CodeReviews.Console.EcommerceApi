using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;

namespace ECommerce.Contracts.TerrenceLGee.Mappings.ProductMappings;

public static class FromDto
{
    extension(CreateProductDto productDto)
    {
        public Product FromCreateProductDto()
        {
            return new Product
            {
                CategoryId = productDto.CategoryId,
                Name = productDto.Name,
                Description = productDto.Description,
                StockQuantity = productDto.StockQuantity,
                UnitPrice = productDto.UnitPrice,
                DiscountPercentage = productDto.DiscountPercentage,
                IsInStock = productDto.IsInStock,
                IsDeleted = productDto.IsDeleted,
                ImageUrl = productDto.ImageUrl
            };
        }
    }

    extension(UpdateProductDto productDto)
    {
        public Product FromUpdateProductDto()
        {
            return new Product
            {
                Id = productDto.Id,
                CategoryId = productDto.CategoryId,
                Name = productDto.Name,
                Description = productDto.Description,
                StockQuantity = productDto.StockQuantity,
                DiscountPercentage = productDto.DiscountPercentage,
                IsInStock = productDto.IsInStock,
                IsDeleted = productDto.IsDeleted,
                ImageUrl = productDto.ImageUrl
            };
        }
    }
}
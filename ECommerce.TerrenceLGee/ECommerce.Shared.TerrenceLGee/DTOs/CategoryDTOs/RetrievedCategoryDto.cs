using ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;

namespace ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;

public class RetrievedCategoryDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
}
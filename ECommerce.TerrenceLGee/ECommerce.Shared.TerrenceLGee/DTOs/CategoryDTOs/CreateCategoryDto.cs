using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Category name is required.")]
    [MaxLength(100, ErrorMessage = "Category name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Category description cannot exceed 500 characters.")]
    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;

public class UpdateCategoryDto
{
    [Required(ErrorMessage = "Category Id is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500, ErrorMessage = "Category description cannot exceed 500 characters.")]
    public string? Description { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
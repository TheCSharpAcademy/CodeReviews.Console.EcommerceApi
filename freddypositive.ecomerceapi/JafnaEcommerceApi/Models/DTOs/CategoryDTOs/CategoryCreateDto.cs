using System.ComponentModel.DataAnnotations;

namespace JafnaEcommerceApi.Models.DTOs.CategoryDTOs;
public class CategoryCreateDto
{
    [Required]
    public string Name { get; set; }
}

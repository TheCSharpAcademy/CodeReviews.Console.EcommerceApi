using System.ComponentModel.DataAnnotations;

namespace JafnaEcommerceApi.Models.DTOs.CategoryDTOs;
public class CategoryUpdateDto
{
    [Required]
    public string Name { get; set; }
}

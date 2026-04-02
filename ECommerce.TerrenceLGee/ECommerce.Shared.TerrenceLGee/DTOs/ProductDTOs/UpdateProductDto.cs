using System.ComponentModel.DataAnnotations;

namespace ECommerce.Shared.TerrenceLGee.DTOs.ProductDTOs;

public class UpdateProductDto
{
    [Required(ErrorMessage = "Product Is is required.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Category Id is required.")]
    public int CategoryId { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [MaxLength(ErrorMessage = "Product name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "Product description cannot exceed 1000 characters.")]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Product stock quantity is required.")]
    [Range(0, 5000, ErrorMessage = "Product stock quantity must be between 0 and the maximum capacity of our warehouse which is 5000.")]
    public int StockQuantity { get; set; }

    [Required(ErrorMessage = "Discount percentage is required.")]
    [Range(0, 100, ErrorMessage = "Discount percentage must be between 0% and 100%.")]
    public int DiscountPercentage { get; set; }

    [Required(ErrorMessage = "Information in reqgards to whether we still carry this product or not is required.")]
    public bool IsDeleted { get; set; }

    [Required(ErrorMessage = "Product stock status in our warehouse is required.")]
    public bool IsInStock { get; set; }

    [Url]
    public string? ImageUrl { get; set; }
}
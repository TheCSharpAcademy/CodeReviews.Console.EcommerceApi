namespace ECommerce.Shared.TerrenceLGee.DTOs.CategoryDTOs;

public class RetrievedCategorySummaryForAdminDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
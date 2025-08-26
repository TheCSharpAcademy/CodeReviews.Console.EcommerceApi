namespace EcommerceAPIDemo.Data.DTOs;

public class GameProductResponseDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<int> CategoryIds { get; } = [];
    public List<int> SalesIds { get; } = [];
    public string Developer { get; set; }
    public string Publisher { get; set; }
    public DateTime ReleaseDate { get; set; }
    public double Price { get; set; }
    public double FileSize { get; set; }
    public string SystemRequirements { get; set; }
}

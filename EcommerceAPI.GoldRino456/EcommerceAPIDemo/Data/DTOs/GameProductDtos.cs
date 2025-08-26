namespace EcommerceAPIDemo.Data.DTOs;

public class NewGameDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<int>? GameCategoryIds { get; set; }
    public string Developer { get; set; }
    public string Publisher { get; set; }
    public DateTime ReleaseDate { get; set; }
    public double Price { get; set; }
    public double FileSize { get; set; }
    public string SystemRequirements { get; set; }
}

public class ExistingGameDto
{
    public string Title { get; set; }
    public string Description { get; set; }
    public List<int>? GameCategoryIds { get; set; }
    public string Developer { get; set; }
    public string Publisher { get; set; }
    public DateTime ReleaseDate { get; set; }
    public double FileSize { get; set; }
    public string SystemRequirements { get; set; }
}

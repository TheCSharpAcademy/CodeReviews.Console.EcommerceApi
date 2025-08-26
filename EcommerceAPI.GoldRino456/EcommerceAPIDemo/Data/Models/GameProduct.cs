namespace EcommerceAPIDemo.Data.Models;

public class GameProduct
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public List<GameCategory> Categories { get; } = [];
    public List<Sale> Sales { get; } = [];
    public string Developer { get; set; }
    public string Publisher { get; set; }
    public DateTime ReleaseDate { get; set; }
    public double Price { get; set; }
    public double FileSize { get; set; }
    public string SystemRequirements { get; set; }
}

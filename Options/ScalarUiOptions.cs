namespace ECommerceApp.RyanW84.Options;

public class ScalarUiOptions
{
    public string? Title { get; set; }
    public string? Theme { get; set; } = "blueplanet"; // blueplanet | purple | default
    public string? Layout { get; set; } = "modern"; // modern | classic
    public bool DarkMode { get; set; } = true;
    public bool HideModels { get; set; }
    public bool ShowSidebar { get; set; } = true;
    public bool DefaultOpenAllTags { get; set; } = true;
    public string? SearchHotKey { get; set; } = "k";
    public string? CustomCss { get; set; }
}

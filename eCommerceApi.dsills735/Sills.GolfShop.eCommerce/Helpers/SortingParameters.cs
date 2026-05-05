namespace Sills.GolfShop.eCommerceAPI.Helpers;

public class ProductParameters
{
    public string? name { get; set; }
    public decimal? minPrice { get; set; }
    public decimal? maxPrice { get; set; }

    public string? sortBy { get; set; } = null;
}

public class CategoryParameters
{
    public string? name { get; set; }
    public string? sortBy { get; set; } = null;
}
 public class SalesParameters
{
    public string? sortBy { get; set; } = null;
}

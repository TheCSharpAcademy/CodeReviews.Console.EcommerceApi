namespace kilozdazolik.Ecommerce.API.Features.Products;

public class ProductParameters
{
    public int PageIndex { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? SearchTerm { get; set; }  
    public decimal? MinPrice { get; set; }   
    public decimal? MaxPrice { get; set; } 
    public Guid? CategoryId { get; set; }  

    // possible values: "price_asc", "price_desc", "name"
    public string? SortBy { get; set; }
}
namespace kilozdazolik.Ecommerce.API.Features.Categories
{
    public class CategoryParameters
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string? SearchTerm { get; set; } 
        public string? SortBy { get; set; }     
    }
}

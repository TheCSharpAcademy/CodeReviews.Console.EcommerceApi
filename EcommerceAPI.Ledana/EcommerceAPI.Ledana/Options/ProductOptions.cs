using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Ledana.Options
{
    public class ProductOptions
    {
        [FromQuery(Name = "name")]
        public string? Name { get; set; }
        [FromQuery(Name = "category")]
        public string? Category { get; set; }
        [FromQuery(Name = "stock")]
        public int? Stock { get; set; }
        [FromQuery(Name = "price")]
        public decimal? Price { get; set; }
        [FromQuery(Name = "page_number")]
        public int PageNumber { get; set; } = 1;
        [FromQuery(Name = "page_size")]
        public int PageSize { get; set; } = 100;
        [FromQuery(Name = "sort_by")]
        public string SortBy { get; set; } = "id";
        [FromQuery(Name = "sort_order")]
        public string SortOrder { get; set; } = "ASC";
    }
}

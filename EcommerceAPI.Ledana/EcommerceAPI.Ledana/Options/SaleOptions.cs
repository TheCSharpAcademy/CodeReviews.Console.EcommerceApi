using Microsoft.AspNetCore.Mvc;

namespace EcommerceAPI.Ledana.Options
{
    public class SaleOptions
    {
        [FromQuery(Name = "product_name")]
        public string? ProductName { get; set; }
        [FromQuery(Name = "category_name")]
        public string? CategoryName { get; set; }
        [FromQuery(Name = "total_price")]
        public decimal? TotalPrice { get; set; }
        [FromQuery(Name = "date")]
        public DateTime? Date { get; set; }
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

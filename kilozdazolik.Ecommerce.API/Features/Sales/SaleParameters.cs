namespace kilozdazolik.Ecommerce.API.Features.Sales
{
    public class SaleParameters
    {
        public int PageIndex { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public DateTime? FromDate { get; set; } 
        public DateTime? ToDate { get; set; }   
        public decimal? MinAmount { get; set; } 
        public Guid? ProductId { get; set; }    
        public string? SortBy { get; set; }
    }
}

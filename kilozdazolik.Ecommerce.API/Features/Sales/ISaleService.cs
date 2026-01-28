namespace kilozdazolik.Ecommerce.API.Features.Sales
{
    public interface ISaleService
    {
        Task<SaleDto> CreateSaleAsync(CreateSaleDto createSaleDto);
        Task<SaleDto?> GetSaleByIdAsync(Guid saleId);
        Task<IEnumerable<SaleDto>> GetSalesAsync(SaleParameters parameters);
    }
}

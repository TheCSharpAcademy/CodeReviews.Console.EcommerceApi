using ECommerce.API.Models;

namespace ECommerce.API.Interfaces;

public interface ISaleRepository
{
    public Task PostSale(Sale sale);
    public IQueryable<Sale> GetSales();
    public Task<Sale?> GetSaleByIdAsync(int saleId);
    public Task DeleteSaleAsync(Sale sale);
}
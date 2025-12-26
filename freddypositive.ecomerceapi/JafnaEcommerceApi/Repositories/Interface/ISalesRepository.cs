using JafnaEcommerceApi.Models.Entities;
using System.Linq;

namespace JafnaEcommerceApi.Repositories;

public interface ISaleRepository
{
    Task<int> CreateSale(decimal totalSalePrice);
    Task CreateSaleDetails(List<SaleDetail> saleEntity);
    IQueryable<Sale> GetSalesQuery();
    IQueryable<SaleDetail> GetSaleDetailsQuery(int saleId);
}

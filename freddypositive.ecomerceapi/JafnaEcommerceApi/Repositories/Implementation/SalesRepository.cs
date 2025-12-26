using JafnaEcommerceApi.Data;
using JafnaEcommerceApi.Models.Entities;

namespace JafnaEcommerceApi.Repositories;

public class SalesRepository : ISaleRepository
{
    public readonly JafnaDbContext _jaffnaDbContext;

    public SalesRepository(JafnaDbContext jaffnaDbContext)
    {
        _jaffnaDbContext = jaffnaDbContext;
    }

    public async Task<int> CreateSale(decimal totalSalePrice)
    {
        var sale = new Sale
        {
            TotalPrice = totalSalePrice
        };

        _jaffnaDbContext.Add(sale);

        await _jaffnaDbContext.SaveChangesAsync();

        return sale.Id;
    }

    public async Task CreateSaleDetails(List<SaleDetail> saleEntity)
    {
        _jaffnaDbContext.AddRange(saleEntity);

        await _jaffnaDbContext.SaveChangesAsync();
    }

    public IQueryable<Sale> GetSalesQuery()
    {
        return _jaffnaDbContext.sale.AsQueryable();
    }

    public IQueryable<SaleDetail> GetSaleDetailsQuery(int saleId)
    {
        return _jaffnaDbContext.sale_details.Where(sd => sd.SaleId == saleId).AsQueryable();
    }
}

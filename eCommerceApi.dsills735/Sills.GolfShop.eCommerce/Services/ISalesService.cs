using Microsoft.EntityFrameworkCore;
using Sills.GolfShop.eCommerceAPI.Data;
using Sills.GolfShop.eCommerceAPI.Models;

namespace Sills.GolfShop.eCommerceAPI.Services;

public interface ISalesService
{
    Task<List<Sales>> GetAllSalesAsync();
    Task<Sales> GetSaleByIdAsync(int id);
    Task<Sales> CreateSaleAsync(Sales sale);
    Task DeleteSaleAsync(int id);
    Task UpdateSaleAsync(int id, Sales sale);
    Task<List<Sales>> GetPagedSalesAsync(int pageNumber, int pageSize);
}
public class SalesService : ISalesService
{
    private readonly GolfShopDbContext _context;

    public SalesService(GolfShopDbContext context)
    {
        _context = context;
    }

    
    public async Task<List<Sales>> GetAllSalesAsync()
    {
        return await _context.Sales.ToListAsync();
    }
    
    public async Task<Sales> GetSaleByIdAsync(int id)
    {
        return await _context.Sales.FindAsync(id);
    }
    public async Task<Sales> CreateSaleAsync(Sales sale)
    {
        _context.Sales.Add(sale);
        await _context.SaveChangesAsync();
        return sale;
    }
    public async Task UpdateSaleAsync(int id, Sales sale)
    {
        var existingSale = await _context.Sales.FindAsync(id);
        if (existingSale == null)
        {
            return;
        }
        existingSale.customerName = sale.customerName;
        existingSale.shippingAddress = sale.shippingAddress;
        await _context.SaveChangesAsync();
    }
    public async Task DeleteSaleAsync(int id)
    {
        var sale = await _context.Sales.FindAsync(id);
        if (sale == null)
        {
            return;
        }
        _context.Sales.Remove(sale);
        await _context.SaveChangesAsync();
    }
    public async Task AddProductToSaleAsync(int saleId, int productId)
    {
        var sale = await _context.Sales.FindAsync(saleId);
        var product = await _context.Products.FindAsync(productId);
        if (sale == null || product == null)
        {
            return;
        }
        var productSale = new ProductSales
        {
            Sale = sale,
            Product = product
        };
        _context.ProductSales.Add(productSale);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveProductFromSaleAsync(int saleId, int productId)
    {
        var productSale = await _context.ProductSales
            .FirstOrDefaultAsync(ps => ps.SaleID == saleId && ps.ProductID == productId);
        if (productSale == null)
        {
            return;
        }
        _context.ProductSales.Remove(productSale);
        await _context.SaveChangesAsync();
    }

    public async Task<List<Sales>> GetPagedSalesAsync(int pageNumber, int pageSize)
    {
        return await _context.Sales
            .OrderBy(s => s.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

}

using Ecommerce.Core.Interfaces.Repositories;
using Ecommerce.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data.Repositories;

public class SaleRepository(AppDbContext context) : GenericRepository<Sale>(context), ISaleRepository
{
    private readonly AppDbContext _context = context;
    public async Task<Sale?> GetSaleWithItemsAsync(int id)
    {
        return await _context.Sales
            .Include(s => s.Items)
            .ThenInclude(si => si.Product)
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(s => s.Id == id);
    }
}
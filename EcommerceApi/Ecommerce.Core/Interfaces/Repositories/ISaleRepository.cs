using Ecommerce.Core.Models;

namespace Ecommerce.Core.Interfaces.Repositories;

public interface ISaleRepository : IGenericRepository<Sale>
{
    Task<Sale?> GetSaleWithItemsAsync(int id);
}
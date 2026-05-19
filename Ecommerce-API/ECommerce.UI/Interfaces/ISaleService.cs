using ECommerce.Shared.Models;

namespace ECommerce.UI.Interfaces;

public interface ISaleService
{
    public Task<PagedResponse<SaleDto>> GetSalesAsync(int pageNumber = 1, int pageSize = 10);

    /// <summary>
    ///     Posts a new sale to the API asynchronously
    /// </summary>
    /// <param name="itemIdQuantityPairs">An item ID - quantity pair for each item in the sale</param>
    /// <returns></returns>
    public Task PostSaleAsync(Dictionary<int, int> itemIdQuantityPairs);

    public Task DeleteSaleAsync(int id);
}
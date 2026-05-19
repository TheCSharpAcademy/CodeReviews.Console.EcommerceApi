using ECommerce.API.Models;
using ECommerce.Shared.Models;

namespace ECommerce.API.Interfaces;

public interface ISaleService
{
    public Task<bool?> PostSaleAsync(List<CreateSaleItemDto> saleItems);
    public Task<PagedResponse<SaleDto>> GetSalesAsync(PaginationParams paginationParams);
    public Task<bool?> DeleteSaleByIdAsync(int saleId);
}
using Ecommerce.Core.DTOs.Sale;
using Ecommerce.Core.Utilities;

namespace Ecommerce.Core.Interfaces.Services;

public interface ISaleService
{
    Task<Result<SaleDto>> CreateSaleAsync(CreateSaleDto sale);
    Task<Result<SaleDto>> GetSaleAsync(int id);
    Task<Result<IEnumerable<SaleDto>>> GetAllSalesAsync(PaginationParams paginationParams);
}
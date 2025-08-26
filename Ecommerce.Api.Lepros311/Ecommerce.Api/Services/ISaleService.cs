using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public interface ISaleService
{
    Task<PagedResponse<List<SaleDto>>> GetPagedSales(PaginationParams paginationParams);

    Task<BaseResponse<Sale>> GetSaleById(int id);

    Task<BaseResponse<SaleDto>> CreateSale(WriteSaleDto sale);

    Task<BaseResponse<Sale>> UpdateSale(int id, UpdateSaleDto sale);

    Task<BaseResponse<Sale>> DeleteSale(int id);
}



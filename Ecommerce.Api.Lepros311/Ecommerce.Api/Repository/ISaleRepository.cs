using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Repository;

public interface ISaleRepository
{
    public Task<PagedResponse<List<Sale>>> GetPagedSales(PaginationParams paginationParams);

    public Task<BaseResponse<Sale>> GetSaleById(int id);

    public Task<BaseResponse<Sale>> CreateSale(Sale sale);

    public Task<BaseResponse<Sale>> UpdateSale(Sale updatedSale);

    public Task<BaseResponse<Sale>> DeleteSale(int id);
}

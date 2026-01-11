using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;

namespace ECommerceApp.RyanW84.Interfaces;

public interface ISaleService
{
    Task<ApiResponseDto<Sale>> CreateSaleAsync(ApiRequestDto<Sale> request, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<Sale>> GetSaleByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PaginatedResponseDto<List<Sale>>> GetSalesAsync(SaleQueryParameters parameters, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<Sale>> GetSaleByIdWithHistoricalProductsAsync(int id, CancellationToken cancellationToken = default);
    Task<ApiResponseDto<List<Sale>>> GetHistoricalSalesAsync(CancellationToken cancellationToken = default);

    Task<ApiResponseDto<Sale>> UpdateSaleAsync(
        int id,
        ApiRequestDto<Sale> request,
        CancellationToken cancellationToken = default
    );

    Task<ApiResponseDto<bool>> DeleteSaleAsync(
        int id,
        CancellationToken cancellationToken = default
    );
}

using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;

namespace ECommerceApp.RyanW84.Interfaces.Helpers;

public interface ISaleProcessingHelper
{
    ApiResponseDto<Sale>? ValidateCreateSaleRequest(ApiRequestDto<Sale> request);

    Task<(
        bool IsError,
        List<Product> Data,
        ApiResponseDto<Sale> Error
    )> FetchAndValidateProductsAsync(Sale payload, CancellationToken cancellationToken);

    Task<ApiResponseDto<Sale>> ExecuteSaleTransactionAsync(
        Sale payload,
        List<Product> products,
        CancellationToken cancellationToken
    );
}

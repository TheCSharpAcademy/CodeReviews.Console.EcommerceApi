using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;

public interface ISaleService
{
    Task<Result<RetrievedSaleDto?>> AddSaleAsync(CreateOrderDto order);
    Task<Result<RetrievedSaleDto?>> GetSaleAsync(RequestSaleDto request);
    Task<Result<RetrievedSaleDto?>> GetSaleForAdminAsync(RequestSaleDto request);
    Task<Result<PagedList<RetrievedSaleSummaryDto>>> GetSalesAsync(SaleQueryParams saleQueryParams);
    Task<Result<PagedList<RetrievedSaleSummaryDto>>> GetAllSalesForAdminAsync(SaleQueryParams saleQueryParams);
    Task<Result> AdminUpdateSaleStatusAsync(UpdateSaleStatusDto sale);
    Task<Result> CustomerCancelSaleAsync(CancelSaleDto cancel);
}

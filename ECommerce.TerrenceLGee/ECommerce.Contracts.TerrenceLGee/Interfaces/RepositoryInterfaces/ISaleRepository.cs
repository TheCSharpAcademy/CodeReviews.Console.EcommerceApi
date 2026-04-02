using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.Enums;
using ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;

namespace ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;

public interface ISaleRepository
{
    Task<Sale?> AddSaleAsync(Sale sale);
    Task<Sale?> GetSaleAsync(int saleId, string? customerId);
    Task<Sale?> GetSaleForAdminAsync(int saleId);
    Task<(bool, SaleStatus)> AdminUpdateSaleStatusAsync(int saleId, SaleStatus status);
    Task<(bool, SaleStatus)> CustomerCancelSaleAsync(int saleId, string? customerId);
    Task<PagedList<Sale>> GetSalesAsync(SaleQueryParams saleQueryParams);
    Task<PagedList<Sale>> GetAllSalesForAdminAsync(SaleQueryParams saleQueryParams);
}

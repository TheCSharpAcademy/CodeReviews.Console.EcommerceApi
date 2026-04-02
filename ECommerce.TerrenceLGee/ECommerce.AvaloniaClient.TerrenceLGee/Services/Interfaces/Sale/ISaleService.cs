using ECommerce.AvaloniaClient.TerrenceLGee.Data.Models.Sale;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;
using System.Threading.Tasks;

namespace ECommerce.AvaloniaClient.TerrenceLGee.Services.Interfaces.Sale;

public interface ISaleService
{
    Task<SaleData?> CreateOrderAsync(CreateOrderDto sale);
    Task<SaleData?> GetSaleForCustomerAsync(int saleId);
    Task<SaleData?> GetSaleForAdminAsync(int saleId);
    Task<SalesRoot?> GetSalesForCustomerAsync(SaleQueryParams queryParams);
    Task<SalesRoot?> GetSalesForAdminAsync(SaleQueryParams queryParams);
    Task<(bool, string?)> AdminUpdateSaleStatusAsync(int saleId, UpdateSaleStatusDto saleStatus);
    Task<(bool, string?)> CustomerCancelSaleAsync(int saleId);
}

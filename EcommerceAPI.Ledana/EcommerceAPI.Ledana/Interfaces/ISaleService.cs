using EcommerceAPI.Ledana.DTOs;
using EcommerceAPI.Ledana.Models;
using EcommerceAPI.Ledana.Options;

namespace EcommerceAPI.Ledana.Interfaces
{
    public interface ISaleService
    {
        Task<ApiResponseDto<Sale>> CreateSale(SaleDto sale);
        Task<ApiResponseDto<List<SaleProductViewDto>>> GetAllSales(SaleOptions saleOptions);
        Task<ApiResponseDto<List<SaleProductViewDto>>> GetAllSales();
        Task<ApiResponseDto<SaleProductViewDto>> GetSaleById(int id);
    }
}

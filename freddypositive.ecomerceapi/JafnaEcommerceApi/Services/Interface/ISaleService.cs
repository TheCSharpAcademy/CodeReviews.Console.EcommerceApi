using JafnaEcommerceApi.Models.DTOs.SaleDTOs;
using JafnaEcommerceApi.Models.DTOs.PaginationDTOs;

namespace JafnaEcommerceApi.Services;

public interface ISaleService
{
    Task CreateAsync(SaleCreateDto saleCreateDto);
    Task<PagedResponse<SaleDto>> GetAllSaleAsync(int pageNumber, int pageSize);
    Task<PagedResponse<SaleDetailDto>> GetSaleDetailsAsync(int saleId, int pageNumber, int pageSize);
}

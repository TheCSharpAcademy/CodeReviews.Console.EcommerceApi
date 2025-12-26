using JafnaEcommerceApi.Models.DTOs.SaleDTOs;
using JafnaEcommerceApi.Services;
using Microsoft.AspNetCore.Mvc; 
using JafnaEcommerceApi.Models.DTOs.APIResponse;
using JafnaEcommerceApi.Models.DTOs.PaginationDTOs;

namespace JafnaEcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] SaleCreateDto saleCreateDto)
    {
        await _saleService.CreateAsync(saleCreateDto);
        return Ok(ApiResponse<object>.Success(null, "Sale created successfully"));
    }

    [HttpGet]
    public async Task<ActionResult> GetAllSaleAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var saleList = await _saleService.GetAllSaleAsync(pageNumber, pageSize);
        return Ok(ApiResponse<PagedResponse<SaleDto>>.Success(saleList, "Sale obtained successfully"));
    }

    [HttpGet("{saleId}/details")]
    public async Task<ActionResult> GetSaleDetailsAsync([FromRoute] int saleId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var detailsList = await _saleService.GetSaleDetailsAsync(saleId, pageNumber, pageSize);
        return Ok(ApiResponse<PagedResponse<SaleDetailDto>>.Success(detailsList, "Sale details obtained successfully"));
    }
}

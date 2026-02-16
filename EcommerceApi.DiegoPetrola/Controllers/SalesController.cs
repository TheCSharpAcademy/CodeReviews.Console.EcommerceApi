using EcommerceApi.Models.DTOs;
using EcommerceApi.Services;
using EcommerceApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace EcommerceApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(SalesService service, IOptions<PaginationSettings> options) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<SaleDto>> GetSale(int id)
    {
        var res = await service.GetSale(id);
        return this.ToActionResult(res);
    }

    [HttpGet()]
    public async Task<ActionResult<List<SaleDto>>> GetSalePage([FromQuery] int? pageSize, [FromQuery] int pageNumber = 0)
    {
        int finalSize = Math.Min(pageSize ?? options.Value.DefaultPageSize, options.Value.MaxPageSize);
        var res = await service.GetSalesByPage(pageNumber, finalSize);
        return this.ToActionResult(res);
    }

    [HttpPost]
    public async Task<ActionResult<SaleDto>> CreateSale(CreateSaleDto dto)
    {
        var res = await service.CreateSale(dto);
        return this.ToActionResult(res);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<SaleDto>> DeleteSale(int id)
    {
        var res = await service.DeleteSale(id);
        return this.ToActionResult(res);
    }
}

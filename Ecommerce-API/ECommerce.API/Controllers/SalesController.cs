using ECommerce.API.Interfaces;
using ECommerce.API.Models;
using ECommerce.Shared.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(ISaleService saleService) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> PostSale([FromBody] List<CreateSaleItemDto> saleItems)
    {
        var success = await saleService.PostSaleAsync(saleItems);
        return success switch
        {
            null => NotFound(),
            true => NoContent(),
            false => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<Sale>>> GetItemsAsync([FromQuery] PaginationParams paginationParams)
    {
        var pagedResponse = await saleService.GetSalesAsync(paginationParams);
        if (pagedResponse.TotalRecords == 0) return NotFound();
        return Ok(pagedResponse);
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteSaleAsync([FromQuery] int saleId)
    {
        var success = await saleService.DeleteSaleByIdAsync(saleId);
        return success switch
        {
            null => NotFound(),
            true => NoContent(),
            false => StatusCode(StatusCodes.Status500InternalServerError)
        };
    }
}
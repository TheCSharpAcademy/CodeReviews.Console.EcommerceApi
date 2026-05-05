using Sills.GolfShop.eCommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using Sills.GolfShop.eCommerceAPI.Models;
using Sills.GolfShop.eCommerceAPI.Helpers;

namespace Sills.GolfShop.eCommerceAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController(ISalesService salesService) : ControllerBase
{
    private readonly ISalesService _salesService = salesService;

    [HttpGet]
    public async Task<ActionResult<List<Sales>>> GetAllSales([FromQuery] PaginationParameters param)
    {
        var pagedSales = await _salesService
            .GetPagedSalesAsync(param.PageNumber, param.PageSize);
        return Ok(pagedSales);
    }
    [HttpGet("{id}")]
    public async Task<ActionResult<Sales>> GetSaleById(int id)
    {
        var sale = await _salesService.GetSaleByIdAsync(id);
        if (sale == null)
        {
            return NotFound();
        }
        return Ok(sale);
    }
    [HttpPost]
    public async Task<ActionResult<Sales>> CreateSale(Sales sale)
    {
        var createdSale = await _salesService.CreateSaleAsync(sale);
        return CreatedAtAction(nameof(GetSaleById), new { id = createdSale.Id }, createdSale);
    }
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateSale(int id, Sales sale)
    {
        var existingSale = await _salesService.GetSaleByIdAsync(id);
        if (existingSale == null)
        {
            return NotFound();
        }
        await _salesService.UpdateSaleAsync(id, sale);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(int id)
    {
        var existingSale = await _salesService.GetSaleByIdAsync(id);
        if (existingSale == null)
        {
            return NotFound();
        }
        await _salesService.DeleteSaleAsync(id);
        return NoContent();
    }

}

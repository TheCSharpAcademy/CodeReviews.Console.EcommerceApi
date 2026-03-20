using Ecommerce.API.davetn657.Data;
using Ecommerce.API.davetn657.Data.Dtos;
using Ecommerce.API.davetn657.Models;
using Ecommerce.API.davetn657.Services;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.API.davetn657.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;

    public SalesController(ISaleService saleService)
    {
        _saleService = saleService;
    }

    [HttpGet]
    public ActionResult<PagedResponse<Sale>> AllSales()
    {
        return Ok(_saleService.AllSales(new PaginationParams()));
    }
    [HttpGet("id/{id}")]
    public ActionResult<Sale> SaleById(int id)
    {
        return Ok(_saleService.SaleById(id));
    }
    [HttpPost]
    public ActionResult<SalesResponseDto> CreateSale(CreateSalesDto sale)
    {
        return Ok(_saleService.CreateSale(sale));
    }
}
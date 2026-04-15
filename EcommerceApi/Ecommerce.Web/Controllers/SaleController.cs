using Asp.Versioning;
using Ecommerce.Core.DTOs.Sale;
using Ecommerce.Core.Interfaces.Services;
using Ecommerce.Core.Utilities;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Web.Controllers;

[ApiController]
[Route("api/v{v:apiVersion}/[controller]")]
public class SaleController(ISaleService saleService, IValidator<CreateSaleDto> createValidator) : ControllerBase
{
    private readonly ISaleService _saleService = saleService;
    private readonly IValidator<CreateSaleDto> _createValidator = createValidator;

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var sales = await _saleService.GetAllSalesAsync(paginationParams);
        return Ok(sales.Data);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var sale = await _saleService.GetSaleAsync(id);
        return sale.IsSuccess ? Ok(sale.Data) : NotFound(sale.Message);
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(CreateSaleDto sale)
    {
        var validationResult = await _createValidator.ValidateAsync(sale);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(x => $"{x.PropertyName} => {x.ErrorMessage}").ToList();
            return BadRequest(new {Errors = errors});
        }
        var result = await _saleService.CreateSaleAsync(sale);
        return result.IsSuccess ? Ok(result.Data) : BadRequest(result.Message);
    }
}
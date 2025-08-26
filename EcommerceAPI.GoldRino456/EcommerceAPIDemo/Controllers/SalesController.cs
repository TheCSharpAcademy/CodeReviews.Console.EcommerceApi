using EcommerceAPIDemo.Data.DTOs;
using EcommerceAPIDemo.Data.Models;
using EcommerceAPIDemo.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EcommerceAPIDemo.Controllers;

[ApiController]
[Route("api/sales")]
public class SalesController : ControllerBase
{
    private readonly ISalesService _salesService;
    public SalesController(ISalesService salesService)
    {
        _salesService = salesService;
    }

    [HttpGet]
    public async Task<ActionResult<PagedResponse<SaleResponseDto>>> GetAllSalesAsync([FromQuery] PaginationParams pagination, [FromQuery] SalesFilterParams filters)
    {
        var query = _salesService.GetAllSales();

        if (query == null)
        {
            return NoContent();
        }

        if(filters.GameProductId.HasValue)
        {
            query = query.Where(s => s.GamesPurchased.Any(p => p.Id == filters.GameProductId));
        }
        if(filters.TransactionDate.HasValue)
        {
            query = query.Where(s => DateOnly.FromDateTime(s.TransactionDate).Equals(filters.TransactionDate));
        }
        if(filters.MinTransactionValue.HasValue)
        {
            query = query.Where(s => s.ActualTransactionValue >= filters.MinTransactionValue);
        }
        if (filters.MaxTransactionValue.HasValue)
        {
            query = query.Where(s => s.ActualTransactionValue < filters.MaxTransactionValue);
        }

        var totalRecords = await query.CountAsync();
        var items = await query.Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();

        var responseItems = items.Select(x => _salesService.ConvertSaleObjToResponseDto(x)).ToList();

        var pagedResponse = new PagedResponse<SaleResponseDto>(responseItems, pagination.PageNumber, pagination.PageSize, totalRecords);

        return Ok(pagedResponse);
    }

    [HttpGet("{saleId}")]
    public async Task<ActionResult<SaleResponseDto>> GetSaleAsync(int saleId)
    {
        var selectedSale = await _salesService.GetSale(saleId);

        if (selectedSale == null)
        {
            return NotFound($"Sale with id {saleId} was not found.");
        }

        var response = _salesService.ConvertSaleObjToResponseDto(selectedSale);
        return Ok(response);
    }

    [HttpPost]
    public async Task<ActionResult<SaleResponseDto>> CreateSaleAsync(SaleDto dto)
    {
        var newSale = await _salesService.CreateSale(dto);
        return Ok(_salesService.ConvertSaleObjToResponseDto(newSale));
    }

    [HttpPut("{saleId}")]
    public async Task<ActionResult<SaleResponseDto>> ProcessSaleRefundAsync(int saleId, [FromQuery]double refundAmount)
    {
        var selectedSale = await _salesService.GetSale(saleId);

        //Check for valid input parameters
        if (selectedSale == null)
        {
            return NotFound($"Sale with id {saleId} was not found.");
        }
        if(selectedSale.IsRefund)
        {
            return BadRequest($"Sale with id {saleId} was already refunded. No amount in transaction left to refund.");
        }
        if(refundAmount <= 0)
        {
            return BadRequest($"Amount to refund cannot be a negative value or equal to zero.");
        }
        if(refundAmount > selectedSale.ActualTransactionValue)
        {
            return BadRequest($"Sale with id {saleId} has transaction value that is less than the refund amount of {refundAmount}. Cannot refund more than what was sold.");
        }

        //Full or Partial Refund?
        if(refundAmount == selectedSale.ActualTransactionValue)
        {
            await _salesService.RefundExistingSale(saleId);
        }
        else
        {
            await _salesService.RefundPartialExistingSale(saleId, refundAmount);
        }

        return Ok(_salesService.ConvertSaleObjToResponseDto(selectedSale));
    }
}

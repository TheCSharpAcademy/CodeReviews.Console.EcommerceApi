using ECommerce.Api.TerrenceLGee.Controllers.Helpers;
using ECommerce.Api.TerrenceLGee.Responses;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Entities.TerrenceLGee.Models;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;
using ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerce.Api.TerrenceLGee.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalesController : ControllerBase
{
    private readonly ISaleService _saleService;
    private readonly UserManager<ApplicationUser> _userManager;

    public SalesController(ISaleService saleService, UserManager<ApplicationUser> userManager)
    {
        _saleService = saleService;
        _userManager = userManager;
    }

    [HttpPost("checkout")]
    [Authorize(Roles = "customer")]
    public async Task<ActionResult<ApiResponse<RetrievedSaleDto?>>> AddSale([FromBody] CreateOrderDto order)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedSaleDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        order.CustomerId = userId;

        var response = ApiResponse<RetrievedSaleDto?>.GetEmptyResponse;

        var result = await _saleService.AddSaleAsync(order);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedSaleDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedSaleDto?>(201, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("{id:int}")]
    [Authorize(Roles = "customer")]
    public async Task<ActionResult<ApiResponse<RetrievedSaleDto?>>> GetSale([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedSaleDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var requestDto = new RequestSaleDto
        {
            SaleId = id,
            CustomerId = userId
        };

        var response = ApiResponse<RetrievedSaleDto?>.GetEmptyResponse;

        var result = await _saleService.GetSaleAsync(requestDto);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedSaleDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedSaleDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("admin/{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<RetrievedSaleDto?>>> GetSaleForAdmin([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<RetrievedSaleDto?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var requestDto = new RequestSaleDto
        {
            SaleId = id
        };

        var response = ApiResponse<RetrievedSaleDto?>.GetEmptyResponse;

        var result = await _saleService.GetSaleForAdminAsync(requestDto);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<RetrievedSaleDto?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<RetrievedSaleDto?>(200, result.Value);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet]
    [Authorize(Roles = "customer")]
    public async Task<ActionResult<ApiResponsePaged<RetrievedSaleSummaryDto>>> GetSales([FromQuery] SaleQueryParams saleQueryParams)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationPagedAsync<RetrievedSaleSummaryDto>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        saleQueryParams.CustomerId = userId;

        var result = await _saleService.GetSalesAsync(saleQueryParams);

        var response = new ApiResponsePaged<RetrievedSaleSummaryDto>(200, result.Value!);

        return StatusCode(response.StatusCode, response);
    }

    [HttpGet("admin")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponsePaged<RetrievedSaleSummaryDto>>> GetAllSalesForAdmin([FromQuery] SaleQueryParams saleQueryParams)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isUserValid, errorResponse) = await UserValidationPagedAsync<RetrievedSaleSummaryDto>(userId);

        if (!isUserValid)
        {
            return errorResponse;
        }

        var result = await _saleService.GetAllSalesForAdminAsync(saleQueryParams);

        var response = new ApiResponsePaged<RetrievedSaleSummaryDto>(200, result.Value!);

        return StatusCode(response.StatusCode, response);
    }

    [HttpPut("admin/update/{id:int}")]
    [Authorize(Roles = "admin")]
    public async Task<ActionResult<ApiResponse<string?>>> AdminUpdateSaleStatus([FromRoute] int id, [FromBody] UpdateSaleStatusDto updateSaleStatus)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<string?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        updateSaleStatus.SaleId = id;

        var response = ApiResponse<string?>.GetEmptyResponse;

        var result = await _saleService.AdminUpdateSaleStatusAsync(updateSaleStatus);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<string?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, $"Sale {id} status successfully updated to {updateSaleStatus.Status}");

        return StatusCode(response.StatusCode, response);
    }

    [HttpPost("cancel/{id:int}")]
    [Authorize(Roles = "customer")]
    public async Task<ActionResult<ApiResponse<string?>>> CustomerCancelSale([FromRoute] int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        var (isValidUser, errorResponse) = await UserValidationAsync<string?>(userId);

        if (!isValidUser)
        {
            return errorResponse;
        }

        var cancelSale = new CancelSaleDto
        {
            SaleId = id,
            CustomerId = userId
        };

        var response = ApiResponse<string?>.GetEmptyResponse;

        var result = await _saleService.CustomerCancelSaleAsync(cancelSale);

        if (result.IsFailure)
        {
            response = FailureHelper.HandleFailureResult<string?>(result);
            return StatusCode(response.StatusCode, response);
        }

        response = new ApiResponse<string?>(200, $"Sale {id} successfully canceled");

        return StatusCode(response.StatusCode, response);
    }

    private async Task<(bool isUserValid, ActionResult<ApiResponse<T?>>)> UserValidationAsync<T>(string? userId)
    {
        var (isValidUser, errorResponse) = await AuthHelper.IsValidUserAsync<T?>(_userManager, userId);

        if (!isValidUser)
        {
            return (false, StatusCode(errorResponse.StatusCode, errorResponse));
        }

        return (true, null!);
    }

    private async Task<(bool isUserValid, ActionResult<ApiResponsePaged<T>>)> UserValidationPagedAsync<T>(string? userId)
    {
        var (isValidUser, errorResponse) = await AuthHelper.IsValidUserPagedAsync<T?>(_userManager, userId);

        if (!isValidUser)
        {
            return (false, StatusCode(errorResponse.StatusCode, errorResponse));
        }

        return (true, null!);
    }
}

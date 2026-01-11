using System.Net;
using ECommerceApp.RyanW84.Data;
using ECommerceApp.RyanW84.Data.DTO;
using ECommerceApp.RyanW84.Data.Models;
using ECommerceApp.RyanW84.Interfaces;
using Microsoft.EntityFrameworkCore;
using ECommerceApp.RyanW84.Interfaces.Helpers;

namespace ECommerceApp.RyanW84.Services;

public class SaleService(
    ECommerceDbContext db,
    ISaleRepository saleRepository,
    ISaleProcessingHelper saleProcessingHelper,
    ISaleQueryHelper saleQueryHelper
) : ISaleService
{
    private readonly ECommerceDbContext _db = db;
    private readonly ISaleRepository _saleRepository = saleRepository;
    private readonly ISaleProcessingHelper _saleProcessingHelper = saleProcessingHelper;
    private readonly ISaleQueryHelper _saleQueryHelper = saleQueryHelper;

    public async Task<ApiResponseDto<Sale>> CreateSaleAsync(
        ApiRequestDto<Sale> request,
        CancellationToken cancellationToken = default
    )
    {
        var validationError = _saleProcessingHelper.ValidateCreateSaleRequest(request);
        if (validationError != null)
            return validationError;

        var products = await _saleProcessingHelper.FetchAndValidateProductsAsync(request.Payload!, cancellationToken);
        if (products.IsError)
            return products.Error;

        return await _saleProcessingHelper.ExecuteSaleTransactionAsync(
            request.Payload!,
            products.Data,
            cancellationToken
        );
    }

    // Extracted helper methods into ISaleProcessingHelper

    public async Task<ApiResponseDto<Sale>> GetSaleByIdAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        Sale? sale = await _db
            .Sales.AsNoTracking()
            .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
            .FirstOrDefaultAsync(s => s.SaleId == id, cancellationToken);

        return sale is null
            ? ApiResponseDto<Sale>.Failure(HttpStatusCode.NotFound, $"Sale {id} not found.")
            : ApiResponseDto<Sale>.Success(sale);
    }

    public async Task<PaginatedResponseDto<List<Sale>>> GetSalesAsync(
        SaleQueryParameters? parameters,
        CancellationToken cancellationToken = default
    )
    {
        try
        {
            parameters ??= new SaleQueryParameters
            {
                Page = 0,
                PageSize = 0,
                StartDate = null,
                EndDate = null,
                CustomerName = null,
                CustomerEmail = null,
                SortBy = null,
                SortDirection = null
            };
            _saleQueryHelper.NormalizeDateRange(parameters);

            var result = await _saleRepository.GetAllSalesAsync(parameters, cancellationToken);
            return result.RequestFailed
                ? PaginatedResponseDto<List<Sale>>.Failure(
                    result.ResponseCode,
                    result.ErrorMessage,
                    parameters.Page,
                    parameters.PageSize
                )
                : PaginatedResponseDto<List<Sale>>.Success(
                    result.Data,
                    result.CurrentPage,
                    result.PageSize,
                    result.TotalCount
                );
        }
        catch (Exception ex)
        {
            return PaginatedResponseDto<List<Sale>>.Failure(
                HttpStatusCode.InternalServerError,
                $"Failed to retrieve sales: {ex.Message}",
                parameters?.Page ?? 1,
                parameters?.PageSize ?? 10
            );
        }
    }

    // Moved to ISaleQueryHelper

    public async Task<ApiResponseDto<Sale>> GetSaleByIdWithHistoricalProductsAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        Sale? sale = await _db
            .Sales.AsNoTracking()
            .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
            .IgnoreQueryFilters() // Include deleted products
            .FirstOrDefaultAsync(s => s.SaleId == id, cancellationToken);

        if (sale is null)
            return ApiResponseDto<Sale>.Failure(HttpStatusCode.NotFound, $"Sale {id} not found.");

        _saleQueryHelper.FilterHistoricalItems(sale);

        return ApiResponseDto<Sale>.Success(sale);
    }

    public async Task<
        ApiResponseDto<List<Sale>>
    > GetHistoricalSalesAsync(CancellationToken cancellationToken = default)
    {
        List<Sale> list = await _db
            .Sales.AsNoTracking()
            .Include(s => s.SaleItems)
                .ThenInclude(si => si.Product)
            .IgnoreQueryFilters() // Include deleted products
            .ToListAsync(cancellationToken);

        _saleQueryHelper.FilterHistoricalItems(list);

        return ApiResponseDto<List<Sale>>.Success(list);
    }

    public async Task<ApiResponseDto<Sale>> UpdateSaleAsync(
        int id,
        ApiRequestDto<Sale> request,
        CancellationToken cancellationToken = default
    )
    {
        if (request.Payload is null)
            return ApiResponseDto<Sale>.Failure(HttpStatusCode.BadRequest, "Invalid request payload");

        // Ensure route id is enforced
        Sale incoming = request.Payload;
        incoming.SaleId = id;

        // Ensure sale exists
        ApiResponseDto<Sale?> existing = await _saleRepository.GetByIdAsync(id, cancellationToken);
        if (existing.RequestFailed)
            return ApiResponseDto<Sale>.Failure(existing.ResponseCode, existing.ErrorMessage);
        if (existing.Data is null)
            return ApiResponseDto<Sale>.Failure(HttpStatusCode.NotFound, $"Sale {id} not found.");

        // Delegate update to repository (keeps logic centralized)
        ApiResponseDto<Sale> updated = await _saleRepository.UpdateAsync(incoming, cancellationToken);
        return updated;
    }

    public async Task<ApiResponseDto<bool>> DeleteSaleAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        // Ensure sale exists before attempting delete
        ApiResponseDto<Sale?> existing = await _saleRepository.GetByIdAsync(id, cancellationToken);
        if (existing.RequestFailed)
            return ApiResponseDto<bool>.Failure(existing.ResponseCode, existing.ErrorMessage);
        if (existing.Data is null)
            return ApiResponseDto<bool>.Failure(HttpStatusCode.NotFound, $"Sale {id} not found.");

        ApiResponseDto<bool> result = await _saleRepository.DeleteAsync(id, cancellationToken);
        return result;
    }
}

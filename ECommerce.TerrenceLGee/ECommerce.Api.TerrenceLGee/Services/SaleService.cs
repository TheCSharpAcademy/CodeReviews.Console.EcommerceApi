using ECommerce.Contracts.TerrenceLGee.Common.Pagination;
using ECommerce.Contracts.TerrenceLGee.Common.Results;
using ECommerce.Contracts.TerrenceLGee.Interfaces.RepositoryInterfaces;
using ECommerce.Contracts.TerrenceLGee.Interfaces.ServiceInterfaces;
using ECommerce.Contracts.TerrenceLGee.Mappings.SaleMappings;
using ECommerce.Shared.TerrenceLGee.DTOs.OrderDTOs;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleDTOs;
using ECommerce.Shared.TerrenceLGee.DTOs.SaleProductDTOs;
using ECommerce.Shared.TerrenceLGee.Enums;
using ECommerce.Shared.TerrenceLGee.Parameters.SaleParameters;

namespace ECommerce.Api.TerrenceLGee.Services;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly IProductRepository _productRepository;

    public SaleService(ISaleRepository saleRepository, IProductRepository productRepository)
    {
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<Result<RetrievedSaleDto?>> AddSaleAsync(CreateOrderDto order)
    {
        if (order.ShoppingCart.Count == 0)
        {
            return Result<RetrievedSaleDto?>.Fail("Your shopping cart must include at least one item in order to make a sale.", ErrorType.BadRequest);
        }

        var totalBaseAmount = 0.0m;
        var totalDiscountAmount = 0.0m;
        var totalAmount = 0.0m;

        var saleProducts = new List<CreateSaleProductDto>();

        foreach (var item in order.ShoppingCart)
        {
            var product = await _productRepository.GetProductAsync(item.ProductId);

            if (product is null)
            {
                return Result<RetrievedSaleDto?>.Fail($"Product {item.ProductId} not found.", ErrorType.NotFound);
            }

            if (!product.IsInStock)
            {
                return Result<RetrievedSaleDto?>.Fail($"{product.Name} is not in stock.", ErrorType.NotFound);
            }

            if (item.Quantity > product.StockQuantity)
            {
                return Result<RetrievedSaleDto?>.Fail($"You ordered {item.Quantity} of {product.Name}, but currently " +
                    $"there are only {product.StockQuantity} of this product in stock.", ErrorType.BadRequest);
            }

            var baseAmount = item.Quantity * product.UnitPrice;
            var discountAmount = (product.DiscountPercentage / 100.0m) * product.UnitPrice;
            var totalPriceForItem = baseAmount - discountAmount;

            totalBaseAmount += baseAmount;
            totalDiscountAmount += discountAmount;

            product.StockQuantity -= item.Quantity;
            await _productRepository.UpdateProductAsync(product);

            saleProducts.Add(new CreateSaleProductDto
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                Price = product.UnitPrice,
                Discount = discountAmount,
                TotalPrice = totalPriceForItem
            });
        }

        totalAmount = totalBaseAmount - totalDiscountAmount;

        var newSale = new CreateSaleDto
        {
            CustomerId = order.CustomerId,
            TotalBaseAmount = totalBaseAmount,
            TotalDiscountAmount = totalDiscountAmount,
            TotalAmount = totalAmount,
            SaleStatus = SaleStatus.Pending,
            SaleProducts = saleProducts
        };

        var sale = await _saleRepository.AddSaleAsync(newSale.FromCreateSaleDto());

        if (sale is null)
        {
            return Result<RetrievedSaleDto?>.Fail($"Unable to complete sale at this time due to unexpected error.", ErrorType.InternalServerError);
        }

        return Result<RetrievedSaleDto?>.Ok(sale.ToRetrievedSaleDto());
    }

    public async Task<Result<RetrievedSaleDto?>> GetSaleAsync(RequestSaleDto request)
    {
        var sale = await _saleRepository.GetSaleAsync(request.SaleId, request.CustomerId);

        if (sale is null)
        {
            return Result<RetrievedSaleDto?>.Fail($"Unable to retrieve sale {request.SaleId}.", ErrorType.NotFound);
        }

        return Result<RetrievedSaleDto?>.Ok(sale.ToRetrievedSaleDto());
    }

    public async Task<Result<RetrievedSaleDto?>> GetSaleForAdminAsync(RequestSaleDto request)
    {
        var sale = await _saleRepository.GetSaleForAdminAsync(request.SaleId);

        if (sale is null)
        {
            return Result<RetrievedSaleDto?>.Fail($"Unable to retrieve sale {request.SaleId}.", ErrorType.NotFound);
        }

        return Result<RetrievedSaleDto?>.Ok(sale.ToRetrievedSaleDto());
    }

    public async Task<Result<PagedList<RetrievedSaleSummaryDto>>> GetSalesAsync(SaleQueryParams saleQueryParams)
    {
        var sales = await _saleRepository.GetSalesAsync(saleQueryParams);

        return Result<PagedList<RetrievedSaleSummaryDto>>.Ok(new PagedList<RetrievedSaleSummaryDto>(
            sales.Select(s => s.ToRetrievedSaleSummaryDto()),
            sales.TotalEntities,
            saleQueryParams.Page,
            saleQueryParams.PageSize));
    }

    public async Task<Result<PagedList<RetrievedSaleSummaryDto>>> GetAllSalesForAdminAsync(SaleQueryParams saleQueryParams)
    {
        var sales = await _saleRepository.GetAllSalesForAdminAsync(saleQueryParams);

        return Result<PagedList<RetrievedSaleSummaryDto>>.Ok(new PagedList<RetrievedSaleSummaryDto>(
            sales.Select(s => s.ToRetrievedSaleSummaryDto()),
            sales.TotalEntities,
            saleQueryParams.Page,
            saleQueryParams.PageSize));
    }

    public async Task<Result> AdminUpdateSaleStatusAsync(UpdateSaleStatusDto sale)
    {
        var (saleStatusUpdated, status) = await _saleRepository.AdminUpdateSaleStatusAsync(sale.SaleId, sale.Status);

        if (!saleStatusUpdated)
        {
            return Result.Fail($"Sale status update failed.\n" +
                $"You are trying to update the status from {status} to {sale.Status} " +
                $"which is not allowed", ErrorType.BadRequest);
        }

        return Result.Ok();
    }

    public async Task<Result> CustomerCancelSaleAsync(CancelSaleDto cancel)
    {
        var (saleCanceled, status) = await _saleRepository.CustomerCancelSaleAsync(cancel.SaleId, cancel.CustomerId);

        if (!saleCanceled)
        {
            return Result.Fail($"Sale cancellation failed because this sale's status is " +
                $"{status} and not eligible to canceled", ErrorType.BadRequest);
        }

        return Result.Ok();
    }
}

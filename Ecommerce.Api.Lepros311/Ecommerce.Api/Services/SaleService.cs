using Ecommerce.Api.Models;
using Ecommerce.Api.Repository;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public class SaleService : ISaleService
{
    private readonly IProductRepository _productRepository;

    private readonly ISaleRepository _saleRepository;

    public SaleService(IProductRepository productRepository, ISaleRepository saleRepository)
    {
        _productRepository = productRepository;
        _saleRepository = saleRepository;
    }

    public async Task<PagedResponse<List<SaleDto>>> GetPagedSales(PaginationParams paginationParams)
    {
        var response = new PagedResponse<List<Sale>>(data: new List<Sale>(),
                                               pageNumber: paginationParams.Page,
                                               pageSize: paginationParams.PageSize,
                                               totalRecords: 0);
        var responseWithDataDto = new PagedResponse<List<SaleDto>>(data: new List<SaleDto>(),
                                               pageNumber: paginationParams.Page,
                                               pageSize: paginationParams.PageSize,
                                               totalRecords: 0);

        response = await _saleRepository.GetPagedSales(paginationParams);

        if (response.Status == ResponseStatus.Fail)
        {
            responseWithDataDto.Status = response.Status;
            responseWithDataDto.Message = response.Message;
            return responseWithDataDto;
        }

        responseWithDataDto.Data = response.Data.Select(s => new SaleDto
        {
            SaleId = s.SaleId,
            DateAndTimeOfSale = s.DateAndTimeOfSale,
            TotalPrice = s.TotalPrice,
            LineItems = s.LineItems.Select(li => new LineItemDto
            {
                LineItemId = li.LineItemId,
                ProductId = li.ProductId,
                ProductName = li.Product.ProductName,
                Category = li.Product.Category.CategoryName,
                Quantity = li.Quantity,
                UnitPrice = li.UnitPrice,
            }).ToList()
        }).ToList();

        return responseWithDataDto;
    }

    public async Task<BaseResponse<Sale>> GetSaleById(int id)
    {
        return await _saleRepository.GetSaleById(id);
    }

    public async Task<BaseResponse<SaleDto>> CreateSale(WriteSaleDto writeSaleDto)
    {
        var response = new BaseResponse<Sale>();
        var responseWithDataDto = new BaseResponse<SaleDto>();

        if (writeSaleDto.LineItems.Any(li => li.Quantity < 0))
        {
            responseWithDataDto.Status = ResponseStatus.Fail;
            responseWithDataDto.Message = "Quantity must be greater than 0.";
            return responseWithDataDto;
        }

        var incomingProductIds = writeSaleDto.LineItems.Select(li => li.ProductId).Distinct().ToList();
        var duplicateProductIds = writeSaleDto.LineItems
            .GroupBy(li => li.ProductId)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();

        if (duplicateProductIds.Any())
        {
            responseWithDataDto.Status = ResponseStatus.Fail;
            responseWithDataDto.Message = $"Duplicate Product ID(s) in sale: {string.Join(", ", duplicateProductIds)}";
            return responseWithDataDto;
        }

        var validProductIds = await _productRepository.GetAllProductIds();
        var missingProductIds = incomingProductIds
            .Where(id => !validProductIds.Contains(id))
            .ToList();

        if (missingProductIds.Any())
        {
            responseWithDataDto.Status = ResponseStatus.Fail;
            responseWithDataDto.Message = $"Invalid Product ID(s): {string.Join(", ", missingProductIds)}";
            return responseWithDataDto;
        }

        var products = await _productRepository.GetProductsByIds(incomingProductIds);
        var productLookup = products.ToDictionary(p => p.ProductId);

        var lineItems = writeSaleDto.LineItems.Select(li => new LineItem
        {
            ProductId = li.ProductId,
            Quantity = li.Quantity,
            UnitPrice = productLookup[li.ProductId].Price,
            Product = productLookup[li.ProductId]
        }).ToList();

        var newSale = new Sale
        {
            DateAndTimeOfSale = DateTime.UtcNow,
            TotalPrice = lineItems.Sum(li => li.Quantity * li.UnitPrice),
            LineItems = lineItems
        };

        response = await _saleRepository.CreateSale(newSale);

        if (response.Status == ResponseStatus.Fail)
        {
            responseWithDataDto.Status = ResponseStatus.Fail;
            responseWithDataDto.Message = response.Message;
            return responseWithDataDto;
        }

        responseWithDataDto.Status = ResponseStatus.Success;

        var newSaleDto = new SaleDto
        {
            SaleId = newSale.SaleId,
            DateAndTimeOfSale = newSale.DateAndTimeOfSale,
            TotalPrice = newSale.TotalPrice,
            LineItems = newSale.LineItems.Select(li => new LineItemDto
            {
                LineItemId = li.LineItemId,
                ProductId = li.ProductId,
                ProductName = li.Product.ProductName,
                Category = li.Product.Category.CategoryName,
                Quantity = li.Quantity,
                UnitPrice = li.UnitPrice,
            }).ToList()
        };

        responseWithDataDto.Data = newSaleDto;

        return responseWithDataDto;
    }

    public async Task<BaseResponse<Sale>> UpdateSale(int id, UpdateSaleDto updateSaleDto)
    {
        var response = new BaseResponse<Sale>();

        response = await GetSaleById(id);

        if (response.Status == ResponseStatus.Fail)
        {
            return response;
        }

        if (updateSaleDto.LineItems.Any(li => li.Quantity < 0))
        {
            response.Status = ResponseStatus.Fail;
            response.Message = "Quantity must be greater than 0.";
            return response;
        }

        var existingSale = response.Data;
        var existingLineItems = existingSale.LineItems.Where(li => li.LineItemId != null).ToDictionary(li => li.LineItemId);
        var validLineItemIds = existingSale.LineItems.Where(li => li.LineItemId != 0).Select(li => li.LineItemId).ToHashSet();
        var incomingLineItemIds = updateSaleDto.LineItems.Where(li => li.LineItemId.HasValue && li.LineItemId.Value != 0).Select(li => li.LineItemId.Value).ToList();
        var invalidLineItemIds = incomingLineItemIds.Where(id => !validLineItemIds.Contains(id)).ToList();

        if (invalidLineItemIds.Any())
        {
            response.Status = ResponseStatus.Fail;
            response.Message = $"Invalid Line Item ID(s): {string.Join(", ", invalidLineItemIds)}";
            return response;
        }

        var existingProductIds = existingSale.LineItems.Select(li => li.ProductId).ToHashSet();
        var newLineItems = updateSaleDto.LineItems.Where(li => !existingSale.LineItems.Any(e => e.LineItemId == li.LineItemId)).ToList();
        var conflictingProductIds = newLineItems.Select(li => li.ProductId).Where(pid => existingProductIds.Contains(pid)).Distinct().ToList();
        var duplicateProductIdsInNewItems = newLineItems.GroupBy(li => li.ProductId).Where(g => g.Count() > 1).Select(g => g.Key).ToList();
        var allConflictingProductIds = duplicateProductIdsInNewItems.Concat(conflictingProductIds).Distinct().ToList();

        if (allConflictingProductIds.Any())
        {
            response.Status = ResponseStatus.Fail;
            response.Message = $"Cannot create duplicate line items for the same product. Duplicate Product ID(s): {string.Join(", ", allConflictingProductIds)}";
            return response;
        }

        var productIds = updateSaleDto.LineItems.Select(li => li.ProductId).Distinct().ToList();
        var products = await _productRepository.GetProductsByIds(productIds);
        var productLookup = products.ToDictionary(p => p.ProductId);
        var validProductIds = productLookup.Keys.ToHashSet();
        var missingProductIds = productIds.Where(id => !validProductIds.Contains(id)).ToList();

        if (missingProductIds.Any())
        {
            response.Status = ResponseStatus.Fail;
            response.Message = $"Invalid Product ID(s): {string.Join(", ", missingProductIds)}";
            return response;
        }

        foreach (var incomingLineItem in updateSaleDto.LineItems)
        {
            if (incomingLineItem.LineItemId.HasValue && existingLineItems.TryGetValue(incomingLineItem.LineItemId.Value, out var existingLineItem))
            {
                existingLineItem.Quantity = incomingLineItem.Quantity;
                existingLineItem.Product = productLookup[incomingLineItem.ProductId];
            }
            else
            {
                existingSale.LineItems.Add(new LineItem
                {
                    ProductId = incomingLineItem.ProductId,
                    Quantity = incomingLineItem.Quantity,
                    UnitPrice = productLookup[incomingLineItem.ProductId].Price,
                    Product = productLookup[incomingLineItem.ProductId]
                });
            }
        }

        existingSale.DateAndTimeOfSale = DateTime.UtcNow;
        existingSale.TotalPrice = existingSale.LineItems.Sum(li => li.Quantity * li.UnitPrice);

        response = await _saleRepository.UpdateSale(existingSale);

        return response;
    }

    public async Task<BaseResponse<Sale>> DeleteSale(int id)
    {
        var response = new BaseResponse<Sale>();

        response = await _saleRepository.GetSaleById(id);

        if (response.Status == ResponseStatus.Fail)
        {
            return response;
        }

        return await _saleRepository.DeleteSale(id);
    }
}

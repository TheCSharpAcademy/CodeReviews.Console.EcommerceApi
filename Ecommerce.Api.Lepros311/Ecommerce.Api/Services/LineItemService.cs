using Ecommerce.Api.Models;
using Ecommerce.Api.Repository;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public class LineItemService : ILineItemService
{
    private readonly ILineItemRepository _lineItemRepository;

    private readonly ISaleRepository _saleRepository;

    private readonly IProductRepository _productRepository;

    public LineItemService(ILineItemRepository lineItemRepository, ISaleRepository saleRepository, IProductRepository productRepository)
    {
        _lineItemRepository = lineItemRepository;
        _saleRepository = saleRepository;
        _productRepository = productRepository;
    }

    public async Task<PagedResponse<List<LineItemDto>>> GetPagedLineItems(PaginationParams paginationParams)
    {
        var response = new PagedResponse<List<LineItem>>(data: new List<LineItem>(),
                                               pageNumber: paginationParams.Page,
                                               pageSize: paginationParams.PageSize,
                                               totalRecords: 0);
        var responseWithDataDto = new PagedResponse<List<LineItemDto>>(data: new List<LineItemDto>(),
                                               pageNumber: paginationParams.Page,
                                               pageSize: paginationParams.PageSize,
                                               totalRecords: 0);

        response = await _lineItemRepository.GetPagedLineItems(paginationParams);

        if (response.Status == ResponseStatus.Fail)
        {
            responseWithDataDto.Status = response.Status;
            responseWithDataDto.Message = response.Message;
            return responseWithDataDto;
        }

        responseWithDataDto.Data = response.Data.Select(li => new LineItemDto
        {
            LineItemId = li.LineItemId,
            ProductId = li.ProductId,
            ProductName = li.Product.ProductName,
            Category = li.Product.Category.CategoryName,
            Quantity = li.Quantity,
            UnitPrice = li.UnitPrice,
            SaleId = li.SaleId,
        }).ToList();

        return responseWithDataDto;
    }

    public async Task<BaseResponse<LineItem>> GetLineItemById(int id)
    {
        return await _lineItemRepository.GetLineItemById(id);
    }

    public async Task<BaseResponse<LineItemDto>> CreateLineItem(WriteLineItemDto writeLineItemDto)
    {
        var lineItemResponse = new BaseResponse<LineItem>();
        var lineItemResponseWithDataDto = new BaseResponse<LineItemDto>();

        var saleResponse = await _saleRepository.GetSaleById(writeLineItemDto.SaleId);

        if (saleResponse.Status == ResponseStatus.Fail)
        {
            lineItemResponseWithDataDto.Status = ResponseStatus.Fail;
            lineItemResponseWithDataDto.Message = saleResponse.Message;
            return lineItemResponseWithDataDto;
        }

        var productResponse = await _productRepository.GetProductById(writeLineItemDto.ProductId);

        if (productResponse.Status == ResponseStatus.Fail)
        {
            lineItemResponseWithDataDto.Status = ResponseStatus.Fail;
            lineItemResponseWithDataDto.Message = saleResponse.Message;
            return lineItemResponseWithDataDto;
        }

        if (writeLineItemDto.Quantity <= 0)
        {
            lineItemResponseWithDataDto.Status = ResponseStatus.Fail;
            lineItemResponseWithDataDto.Message = "Quantity must be greater than 0.";
            return lineItemResponseWithDataDto;
        }

        var newLineItem = new LineItem
        {
            ProductId = writeLineItemDto.ProductId,
            Quantity = writeLineItemDto.Quantity,
            SaleId = writeLineItemDto.SaleId,
        };

        newLineItem.Product = productResponse.Data;
        newLineItem.Sale = saleResponse.Data;

        lineItemResponse = await _lineItemRepository.CreateLineItem(newLineItem);

        if (lineItemResponse.Status == ResponseStatus.Fail)
        {
            lineItemResponseWithDataDto.Status = ResponseStatus.Fail;
            lineItemResponseWithDataDto.Message = lineItemResponse.Message;
            return lineItemResponseWithDataDto;
        }
        else
        {
            lineItemResponseWithDataDto.Status = ResponseStatus.Success;

            var newlineItemDto = new LineItemDto
            {
                LineItemId = newLineItem.LineItemId,
                ProductId = newLineItem.ProductId,
                ProductName = newLineItem.Product.ProductName,
                Category = newLineItem.Product.Category.ToString(),
                Quantity = newLineItem.Quantity,
                UnitPrice = newLineItem.Product.Price,
                SaleId = newLineItem.SaleId,
            };

            lineItemResponseWithDataDto.Data = newlineItemDto;
        }

        return lineItemResponseWithDataDto;
    }

    public async Task<BaseResponse<LineItem>> UpdateLineItem(int id, WriteLineItemDto writeLineItemDto)
    {
        var lineItemResponse = new BaseResponse<LineItem>();

        lineItemResponse = await GetLineItemById(id);

        if (lineItemResponse.Status == ResponseStatus.Fail)
        {
            return lineItemResponse;
        }

        if (writeLineItemDto.Quantity <= 0)
        {
            lineItemResponse.Status = ResponseStatus.Fail;
            lineItemResponse.Message = "Quantity must be greater than 0.";
            return lineItemResponse;
        }

        var saleResponse = await _saleRepository.GetSaleById(writeLineItemDto.SaleId);

        if (saleResponse.Status == ResponseStatus.Fail)
        {
            lineItemResponse.Status = ResponseStatus.Fail;
            lineItemResponse.Message = saleResponse.Message;
            return lineItemResponse;
        }

        var productResponse = await _productRepository.GetProductById(writeLineItemDto.ProductId);

        if (productResponse.Status == ResponseStatus.Fail)
        {
            lineItemResponse.Status = ResponseStatus.Fail;
            lineItemResponse.Message = productResponse.Message;
            return lineItemResponse;
        }

        var existingLineItem = lineItemResponse.Data;

        existingLineItem.ProductId = writeLineItemDto.ProductId;
        existingLineItem.Quantity = writeLineItemDto.Quantity;
        existingLineItem.UnitPrice = productResponse.Data.Price;
        existingLineItem.SaleId = writeLineItemDto.SaleId;

        lineItemResponse = await _lineItemRepository.UpdateLineItem(existingLineItem);

        return lineItemResponse;
    }

    public async Task<BaseResponse<LineItem>> DeleteLineItem(int id)
    {
        var response = new BaseResponse<LineItem>();

        response = await _lineItemRepository.GetLineItemById(id);

        if (response.Status == ResponseStatus.Fail)
        {
            return response;
        }

        return await _lineItemRepository.DeleteLineItem(id);
    }
}

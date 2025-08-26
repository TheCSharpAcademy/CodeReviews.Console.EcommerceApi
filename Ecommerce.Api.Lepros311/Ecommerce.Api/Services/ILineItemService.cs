using Ecommerce.Api.Models;
using Ecommerce.Api.Responses;

namespace Ecommerce.Api.Services;

public interface ILineItemService
{
    Task<PagedResponse<List<LineItemDto>>> GetPagedLineItems(PaginationParams paginationParams);

    Task<BaseResponse<LineItem>> GetLineItemById(int id);

    Task<BaseResponse<LineItemDto>> CreateLineItem(WriteLineItemDto lineItem);

    Task<BaseResponse<LineItem>> UpdateLineItem(int id, WriteLineItemDto lineItem);

    Task<BaseResponse<LineItem>> DeleteLineItem(int id);
}

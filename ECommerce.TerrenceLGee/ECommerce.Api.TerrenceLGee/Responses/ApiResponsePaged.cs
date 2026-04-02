using ECommerce.Contracts.TerrenceLGee.Common.Pagination;

namespace ECommerce.Api.TerrenceLGee.Responses;

public class ApiResponsePaged<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public PagedList<T> Data { get; set; } = [];
    public int PageNumber { get; set; }
    public int TotalPages { get; set; }
    public int TotalItemsRetrieved { get; set; }
    public int TotalItems { get; set; }
    public List<string> Errors { get; set; } = [];

    public ApiResponsePaged(int statusCode, PagedList<T> data)
    {
        StatusCode = statusCode;
        IsSuccess = true;
        Data = data;
        PageNumber = data.PageNumber;
        TotalPages = data.TotalPages;
        TotalItemsRetrieved = data.Count;
        TotalItems = data.TotalEntities;
        Errors = [];
    }

    public ApiResponsePaged(int statusCode, List<string> errors)
    {
        StatusCode = statusCode;
        IsSuccess = false;
        Errors = errors;
    }
}

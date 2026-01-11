using System.Net;

namespace ECommerceApp.RyanW84.Data.DTO;

public class PaginatedResponseDto<T>
{
    public bool RequestFailed { get; set; }
    public HttpStatusCode ResponseCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public T? Data { get; set; }
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => CurrentPage < TotalPages;
    public bool HasPreviousPage => CurrentPage > 1;

    /// <summary>
    /// Creates a success paginated response.
    /// </summary>
    public static PaginatedResponseDto<T> Success(
        T? data,
        int currentPage,
        int pageSize,
        int totalCount
    )
    {
        return new PaginatedResponseDto<T>
        {
            RequestFailed = false,
            ResponseCode = HttpStatusCode.OK,
            ErrorMessage = string.Empty,
            Data = data,
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = totalCount,
        };
    }

    /// <summary>
    /// Creates a failure paginated response.
    /// </summary>
    public static PaginatedResponseDto<T> Failure(
        HttpStatusCode statusCode,
        string errorMessage,
        int currentPage = 1,
        int pageSize = 10
    )
    {
        return new PaginatedResponseDto<T>
        {
            RequestFailed = true,
            ResponseCode = statusCode,
            ErrorMessage = errorMessage,
            Data = default,
            CurrentPage = currentPage,
            PageSize = pageSize,
            TotalCount = 0,
        };
    }
}

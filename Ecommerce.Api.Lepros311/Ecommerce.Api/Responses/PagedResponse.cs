namespace Ecommerce.Api.Responses;

public class PagedResponse<T>
{
    public ResponseStatus Status { get; set; }

    public string Message { get; set; }

    public T? Data { get; set; }

    public int PageNumber { get; set; }

    public int PageSize { get; set; }

    public int TotalRecords { get; set; }

    public PagedResponse(T? data, int pageNumber, int pageSize, int totalRecords)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
    }
}

namespace ECommerce.Shared.Models;

public class PagedResponse<T>
{
    public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalRecords, int totalPages)
    {
        Data = data;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalRecords = totalRecords;
        TotalPages = totalPages;
    }

    public List<T> Data { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalRecords { get; set; }
    public int TotalPages { get; set; }
}
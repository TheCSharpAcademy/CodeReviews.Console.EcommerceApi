namespace ECommerceApi.JJHH17.Models
{
    public class PagedResponse<T>
    {
        public List<T> Data { get; set; } = new();
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }

        public PagedResponse(List<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            this.Data = data;
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalRecords = totalRecords;
        }
    }
}
using System.Net;

namespace EcommerceAPI.Ledana.DTOs
{
    public class ApiResponseDto<T>
    {
        public bool RequestFailed { get; set; } = false;
        public HttpStatusCode ResponseCode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public T? Data { get; set; }

        public int TotalCount { get; set; }
        public int CurrentPage { get; set; }
        public bool HasNext { get; set; }

        public bool HasPrevious { get; set; }
        public int PageSize { get; set; }
    }
}

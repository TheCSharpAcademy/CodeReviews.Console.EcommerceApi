using System.Net;

namespace ECommerceApp.RyanW84.Data.DTO;

public class ApiResponseDto<T>
{
    public bool RequestFailed { get; set; }
    public HttpStatusCode ResponseCode { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
    public T? Data { get; set; }

    /// <summary>
    /// Creates a success response with the provided data.
    /// </summary>
    public static ApiResponseDto<T> Success(T? data, HttpStatusCode statusCode = HttpStatusCode.OK)
    {
        return new ApiResponseDto<T>
        {
            RequestFailed = false,
            ResponseCode = statusCode,
            ErrorMessage = string.Empty,
            Data = data,
        };
    }

    /// <summary>
    /// Creates an error response with the provided status code and message.
    /// </summary>
    public static ApiResponseDto<T> Failure(HttpStatusCode statusCode, string errorMessage)
    {
        return new ApiResponseDto<T>
        {
            RequestFailed = true,
            ResponseCode = statusCode,
            ErrorMessage = errorMessage,
            Data = default,
        };
    }

    /// <summary>
    /// Passthrough for existing responses or conversion from other responses.
    /// </summary>
    public static ApiResponseDto<T> FromResponse<TSource>(ApiResponseDto<TSource> source, T? data)
    {
        return new ApiResponseDto<T>
        {
            RequestFailed = source.RequestFailed,
            ResponseCode = source.ResponseCode,
            ErrorMessage = source.ErrorMessage,
            Data = data,
        };
    }
}

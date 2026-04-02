namespace ECommerce.Api.TerrenceLGee.Responses;

public class ApiResponse<T>
{
    public int StatusCode { get; set; }
    public bool IsSuccess { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = [];

    public ApiResponse() { }

    public ApiResponse(int statusCode, T? data)
    {
        StatusCode = statusCode;
        IsSuccess = true;
        Data = data;
        Errors = [];
    }

    public ApiResponse(int statusCode, List<string> errors)
    {
        StatusCode = statusCode;
        IsSuccess = false;
        Errors = errors;
    }

    public static ApiResponse<T> GetEmptyResponse => new();
}

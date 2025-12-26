namespace JafnaEcommerceApi.Models.DTOs.APIResponse;

public class ApiResponse<T>
{
    public bool success { get; set; }
    public string message { get; set; }
    public T data { get; set; } 
    public List<string> errors { get; set; }

    public static ApiResponse<T> Success(T data, string message = "Operation successful")
    {
        return new ApiResponse<T>
        {
            success = true,
            message = message,
            data = data,
            errors = null
        };
    }

    public static ApiResponse<T> Failure(List<string> errors, string message = "Operation failed")
    {
        return new ApiResponse<T>
        {
            success = false,
            message = message,
            data = default,
            errors = errors
        };
    }
}
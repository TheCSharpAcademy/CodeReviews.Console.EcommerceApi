using ECommerce.Api.TerrenceLGee.Responses;
using ECommerce.Contracts.TerrenceLGee.Common.Results;

namespace ECommerce.Api.TerrenceLGee.Controllers.Helpers;

public class FailureHelper
{
    public static ApiResponse<T?> HandleFailureResult<T>(Result<T?> result)
    {
        var response = new ApiResponse<T?>();
        return response = result.ErrorType switch
        {
            ErrorType.BadRequest => new ApiResponse<T?>(400, [result.ErrorMessage ?? "Bad request"]),
            ErrorType.NotFound => new ApiResponse<T?>(404, [result.ErrorMessage ?? "Not found"]),
            ErrorType.Conflict => new ApiResponse<T?>(409, [result.ErrorMessage ?? "Conflict"]),
            ErrorType.Unauthorized => new ApiResponse<T?>(401, [result.ErrorMessage ?? "Unauthorized"]),
            _ => new ApiResponse<T?>(500, [result.ErrorMessage ?? "Internal server error"])
        };
    }

    public static ApiResponse<T?> HandleFailureResult<T>(Result result)
    {
        var response = new ApiResponse<T?>();
        return response = result.ErrorType switch
        {
            ErrorType.BadRequest => new ApiResponse<T?>(400, [result.ErrorMessage ?? "Bad request"]),
            ErrorType.NotFound => new ApiResponse<T?>(404, [result.ErrorMessage ?? "Not found"]),
            ErrorType.Conflict => new ApiResponse<T?>(409, [result.ErrorMessage ?? "Conflict"]),
            ErrorType.Unauthorized => new ApiResponse<T?>(401, [result.ErrorMessage ?? "Unauthorized"]),
            _ => new ApiResponse<T?>(500, [result.ErrorMessage ?? "Internal server error"])
        };
    }
}

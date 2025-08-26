namespace Ecommerce.Api.Responses;

public class BaseResponse<T>
{
    public ResponseStatus Status { get; set; }

    public string Message { get; set; }

    public T? Data { get; set; }
}
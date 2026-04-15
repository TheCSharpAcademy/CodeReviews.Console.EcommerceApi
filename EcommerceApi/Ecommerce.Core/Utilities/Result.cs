namespace Ecommerce.Core.Utilities;

public record Result<T>
{
    public bool IsSuccess { get; private init; }
    public T? Data { get; private init; }
    public string? Message { get; private init; }

    public static Result<T> Success(T data)
    {
        return new Result<T> { IsSuccess = true, Data = data };
    }
    
    public static Result<T> Fail(string message)
    {
        return new Result<T> { IsSuccess = false, Message = message };
    }
}

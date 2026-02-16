using System.Diagnostics.CodeAnalysis;

namespace EcommerceApi.Results;

// This Result pattern is largely based on the one from the article:
// www.red-gate.com/simple-talk/development/dotnet-development/the-result-pattern-in-asp-net-core-minimal-apis/
public class Result<T>
{
    public T? Value { get; }
    public ErrorResponse? Error { get; }
    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess { get; }
    private Result(T value)
    {
        Value = value;
        IsSuccess = true;
    }
    private Result(ErrorResponse error)
    {
        Error = error;
        IsSuccess = false;
    }
    public static Result<T> Ok(T value) => new(value);
    private static Result<T> Fail(ErrorType type, string error) => new(new ErrorResponse(error, type));
    public static Result<T> NotFound(string error) => Fail(ErrorType.NotFound, error);
    public static Result<T> Invalid(string error) => Fail(ErrorType.Invalid, error);
    public static Result<T> InternalServerError(string error) => Fail(ErrorType.InternalServerError, error);
    public static implicit operator Result<T>(T value) => Ok(value);
}

public record ErrorResponse(string Error, ErrorType ErrorType);

public enum ErrorType
{
    NotFound,
    Invalid,
    Unauthorized,
    Forbidden,
    Conflict,
    InternalServerError
}



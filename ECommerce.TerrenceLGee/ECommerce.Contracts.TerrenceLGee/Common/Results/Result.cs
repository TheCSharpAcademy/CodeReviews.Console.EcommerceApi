namespace ECommerce.Contracts.TerrenceLGee.Common.Results;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ErrorType ErrorType { get; }
    public string? ErrorMessage { get; }

    protected Result(bool isSuccess, string? errorMessage, ErrorType errorType)
    {
        if (isSuccess && errorMessage is not null && errorType != ErrorType.None)
            throw new InvalidOperationException("Success result cannot have an error message and cannot have an error type other than none");
        if (!isSuccess && errorMessage is null && errorType == ErrorType.None)
            throw new InvalidOperationException("Failure result must have an error message and must have an error type other than one");

        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
        ErrorType = errorType;
    }

    public static Result Ok() => new(true, null, ErrorType.None);
    public static Result Fail(string? message, ErrorType errorType) => new(false, message, errorType);
}

public class Result<T> : Result
{
    public T? Value { get; }

    protected internal Result(T? value, ErrorType errorType)
        : base(isSuccess: true, errorMessage: null, errorType: errorType)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    protected internal Result(string? errorMessage, ErrorType errorType) 
        : base(isSuccess: false, errorMessage: errorMessage, errorType: errorType)
    {
        Value = default;
    }

    public static Result<T> Ok(T? value) => new(value, ErrorType.None);
    public new static Result<T> Fail(string? message, ErrorType errorType) => new(message, errorType);
}

public enum ErrorType
{
    BadRequest,
    Conflict,
    InternalServerError,
    NotFound,
    Unauthorized,
    None
}
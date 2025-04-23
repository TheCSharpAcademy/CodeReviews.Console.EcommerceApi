namespace ExerciseTracker.Helper;

public class Result<T>
{
    public bool Success { get; }
    public string? Message { get; }
    public T? Data { get; }

    private Result(bool success, string message, T? data = default)
    {
        Success = success;
        Message = message;
        Data = data;
    }

    public static Result<T> SuccessResult(T data, string message = "Operation successful") =>
            new(true, message, data);

    public static Result<T> FailResult(string message) =>
        new(false, message, default);
}
namespace Commons;

public class Result<T>
{
    public bool Succeeded { get; }
    public string[] Messages { get; }
    public string? ErrorCode { get; }
    public T? Data { get; }

    protected Result(bool succeeded, T? data, string? message,
        string? errorCode = null)
    {
        Succeeded = succeeded;
        Data = data;
        Messages = message == null ? Array.Empty<string>() : new[] { message };
        ErrorCode = errorCode;
    }

    public static Result<T?> Success(T? data) => new(true, data, null);

    public static Result<T?> Failure(string message, string? errorCode = null)
        => new Result<T?>(false, default, message, errorCode);
}
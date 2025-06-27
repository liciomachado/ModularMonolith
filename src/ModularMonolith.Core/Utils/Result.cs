namespace ModularMonolith.Core.Utils;

public class Result<TValue, TError> where TError : Error
{
    public readonly TValue? Value;
    public readonly TError? Error;

    public bool IsSuccess;
    public bool IsFailure => !IsSuccess;

    private Result(TValue value)
    {
        IsSuccess = true;
        Value = value;
        Error = default;
    }

    private Result(TError error)
    {
        IsSuccess = false;
        Value = default;
        Error = error;
    }

    //happy path
    public static implicit operator Result<TValue, TError>(TValue value) => new Result<TValue, TError>(value);

    //error path
    public static implicit operator Result<TValue, TError>(TError error) => new Result<TValue, TError>(error);

    public Result<TValue, TError> Match(Func<TValue, Result<TValue, TError>> success, Func<TError, Result<TValue, TError>> failure)
    {
        if (IsSuccess)
        {
            return success(Value!);
        }
        return failure(Error!);
    }
}

public abstract record Error(IEnumerable<string> Message);

public record NotFoundError : Error
{
    public NotFoundError(string? message = null) : base(string.IsNullOrEmpty(message) ? [] : [message]) { }

    public NotFoundError(IEnumerable<string> message) : base(message) { }
}

public record BadRequestError : Error
{
    public BadRequestError(string? message = null) : base(string.IsNullOrEmpty(message) ? [] : [message]) { }

    public BadRequestError(IEnumerable<string> message) : base(message) { }
}
namespace Patterns.Modern.Result.Classic;

/// <summary>An expected failure: a machine-readable code and a human message. Not "Error": CA1716 (Visual Basic keyword).</summary>
// Guide: §7.6
public sealed record BusinessError(string Code, string Message);

/// <summary>Creates results. Not static members of Result&lt;T&gt;: CA1000 (static members on a generic type).</summary>
public static class Result
{
    public static Result<T> Success<T>(T value) => new(value, null);

    public static Result<T> Failure<T>(BusinessError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new(default!, error);
    }
}

// Role: Result — a success with a value, or a failure with an error, never both. Reading the wrong one throws.
// A struct: create it only through Result.Success and Result.Failure; default(Result<T>) would look like a success.
public readonly struct Result<T>
{
    private readonly T _value;
    private readonly BusinessError? _error;

    internal Result(T value, BusinessError? error) => (_value, _error) = (value, error);

    public bool IsSuccess => _error is null;

    public T Value => IsSuccess ? _value : throw new InvalidOperationException("A failed result has no value.");

    public BusinessError Error => _error ?? throw new InvalidOperationException("A successful result has no error.");

    /// <summary>Transforms the value if there is one; passes the error through untouched.</summary>
    public Result<TOut> Map<TOut>(Func<T, TOut> mapper)
    {
        ArgumentNullException.ThrowIfNull(mapper);
        return IsSuccess ? Result.Success(mapper(_value)) : Result.Failure<TOut>(_error!);
    }

    /// <summary>Chains another operation that can fail; the first failure stops the chain.</summary>
    public Result<TOut> Bind<TOut>(Func<T, Result<TOut>> next)
    {
        ArgumentNullException.ThrowIfNull(next);
        return IsSuccess ? next(_value) : Result.Failure<TOut>(_error!);
    }

    /// <summary>Handles both outcomes with ordinary code: no try/catch.</summary>
    public TOut Match<TOut>(Func<T, TOut> onSuccess, Func<BusinessError, TOut> onFailure)
    {
        ArgumentNullException.ThrowIfNull(onSuccess);
        ArgumentNullException.ThrowIfNull(onFailure);
        return IsSuccess ? onSuccess(_value) : onFailure(_error!);
    }
}

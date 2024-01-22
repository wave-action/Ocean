using System.ComponentModel;

namespace LibOcc.Diagnostics;

public class Result<A, E>
{
    public A? Value { get; init; }
    public E? Error { get; init; }

    private bool _err;

    private Result(A? value, E? err, bool isErr = false)
    {
        Value = value;
        Error = err;
        _err = isErr;
    }

    public static Result<A, E> Ok(A value) =>
        new(value, default(E));

    public static Result<A, E> Err(E err) =>
        new(default(A), err, true);

    public bool IsErr() => _err;

    public Result<B, E> Map<B>(Func<A, B> f)
    {
        return IsErr() 
            ? Result<B, E>.Err(Error!) 
            : Result<B, E>.Ok(f(Value!));
    }
}
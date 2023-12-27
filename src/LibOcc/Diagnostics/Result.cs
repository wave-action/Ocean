using System.ComponentModel;

namespace LibOcc.Diagnostics;

public class Result<A, E>
{
    public A? Value { get; init; }
    public E? Error { get; init; }

    private Result(A? value, E? err)
    {
        Value = value;
        Error = err;
    }

    public static Result<A, E> Ok(A value) =>
        new(value, default(E));

    public static Result<A, E> Err(E err) =>
        new(default(A), err);

    public bool IsErr() => Error is not null;

    public Result<B, E> Map<B>(Func<A, B> f)
    {
        return IsErr() 
            ? Result<B, E>.Err(Error!) 
            : Result<B, E>.Ok(f(Value!));
    }
}
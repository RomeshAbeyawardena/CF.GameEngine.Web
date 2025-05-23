namespace IDFCR.Shared.Abstractions.Results;

public static class UnitResultCollection
{
    public static IUnitResultCollection<T> FromResult<T>(IEnumerable<T>? result, UnitAction action = UnitAction.Get, bool isSuccess = true)
    {
        return new UnitResultCollection<T>(result, action, isSuccess);
    }

    public static IUnitResultCollection<T> Failed<T>(Exception exception, UnitAction action = UnitAction.None)
    {
        return new UnitResultCollection<T>(null, action, false, exception);
    }
}


public record UnitResultCollection<TResult>(IEnumerable<TResult>? Result = null, UnitAction Action = UnitAction.Get,
    bool IsSuccess = true, Exception? Exception = null) : UnitResult<IEnumerable<TResult>>(Result, Action, IsSuccess, Exception), IUnitResultCollection<TResult>
{
    
}
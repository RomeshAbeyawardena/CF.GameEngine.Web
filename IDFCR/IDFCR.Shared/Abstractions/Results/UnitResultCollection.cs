namespace IDFCR.Shared.Abstractions.Results;

public static class UnitResultCollection
{
    /// <summary>
    /// Provides static factory methods for creating and handling collections of unit results, 
    /// encapsulating operation outcomes, actions performed, and error information for enumerable result sets.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result">The internal payload an instance of <see cref="UnitResultCollection{T}" /> will consume, 
    /// <see cref="Enumerable.ToArray{T}(IEnumerable{T})"/> will be called immediatly against the <see cref="IEnumerable{T}"/>
    /// </param>
    /// <param name="action"></param>
    /// <param name="isSuccess"></param>
    /// <returns></returns>
    public static IUnitResultCollection<T> FromResult<T>(IEnumerable<T>? result, UnitAction action = UnitAction.Get, bool isSuccess = true)
    {
        return new UnitResultCollection<T>(result?.ToArray(), action, isSuccess);
    }

    public static IUnitResultCollection<T> Failed<T>(Exception exception, UnitAction action = UnitAction.None)
    {
        return new UnitResultCollection<T>(null, action, false, exception);
    }
}


public record UnitResultCollection<TResult>(IEnumerable<TResult>? Result = null, UnitAction Action = UnitAction.Get,
    bool IsSuccess = true, Exception? Exception = null, FailureReason? FailureReason = null) 
        : UnitResult<IEnumerable<TResult>>(Result, Action, IsSuccess, Exception, FailureReason), IUnitResultCollection<TResult>
{
    
}
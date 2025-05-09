namespace IDFCR.Shared.Abstractions.Results;

public record UnitResultCollection<TResult>(IEnumerable<TResult>? Result = null, UnitAction Action = UnitAction.None,
    bool IsSuccess = true, Exception? Exception = null) : UnitResult<IEnumerable<TResult>>(Result, Action, IsSuccess, Exception), IUnitResultCollection<TResult>
{
    
}
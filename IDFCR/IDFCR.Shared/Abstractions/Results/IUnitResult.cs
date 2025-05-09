namespace IDFCR.Shared.Abstractions.Results;

public interface IUnitResult : IReadOnlyDictionary<string, object?>
{
    Exception? Exception { get; }
    bool IsSuccess { get; }
    UnitAction Action { get; }
    IUnitResult AddMeta(string key, object? value);
    IUnitResult<T> As<T>();
}

public interface IUnitResult<TResult> : IUnitResult
{
    TResult? Result { get; }
}

public interface IUnitResultCollection<TResult> : IUnitResult<IEnumerable<TResult>?>
{

}

public record UnitResultCollection<TResult>(IEnumerable<TResult>? Result = null, UnitAction Action = UnitAction.None,
    bool IsSuccess = true, Exception? Exception = null) : UnitResult<IEnumerable<TResult>>(Result, Action, IsSuccess, Exception), IUnitResultCollection<TResult>
{
    
}
using System.Diagnostics.CodeAnalysis;

namespace IDFCR.Shared.Abstractions.Results;

public enum FailureReason
{
    None = 0,
    NotFound = 1,
    Conflict = 2,
    ValidationError = 3,
    Unauthorized = 4,
    Forbidden = 5,
    InternalError = 6
}

public interface IUnitResult : IReadOnlyDictionary<string, object?>
{
    FailureReason? FailureReason { get; }
    Exception? Exception { get; }
    bool IsSuccess { get; }
    UnitAction Action { get; }
    IUnitResult AddMeta(string key, object? value);
    IUnitResult<T> As<T>();
}

public interface IUnitResult<TResult> : IUnitResult
{
    TResult? Result { get; }
    [MemberNotNullWhen(true, nameof(Result))]
    bool HasValue { get; }
}

public interface IUnitResultCollection<TResult> : IUnitResult<IEnumerable<TResult>?>
{

}

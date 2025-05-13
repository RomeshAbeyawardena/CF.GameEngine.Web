using System.Diagnostics.CodeAnalysis;

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
    [MemberNotNullWhen(true, nameof(Result))]
    bool HasValue { get; }
}

public interface IUnitResultCollection<TResult> : IUnitResult<IEnumerable<TResult>?>
{

}

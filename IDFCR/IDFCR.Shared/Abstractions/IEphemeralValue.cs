using System.Diagnostics.CodeAnalysis;

namespace IDFCR.Shared.Abstractions;

public interface IEphemeralValue<T>
{
    bool TryGetValue([MaybeNullWhen(false)] out T value);
    bool HasValue { get; }
    T? Value { get; set; }
}

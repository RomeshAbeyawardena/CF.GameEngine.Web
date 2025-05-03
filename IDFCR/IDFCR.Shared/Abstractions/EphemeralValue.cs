using System.Diagnostics.CodeAnalysis;

namespace IDFCR.Shared.Abstractions;

public class EphemeralValue<T> : IEphemeralValue<T>
{
    private readonly Lock _lock = new();
    private T? value;

    public bool TryGetValue([MaybeNullWhen(false)] out T value)
    {
        value = default!;
        using var _ = _lock.EnterScope();
        if (!HasValue)
        {
            return false;
        }

        value = Value!;
        return true;
    }

    public bool HasValue
    {
        get
        {
            using var _ = _lock.EnterScope();
            return value is not null;
        }
    }

    public T? Value
    {
        get
        {
            using var _ = _lock.EnterScope();
            var accessedValue = value;
            value = default;
            return accessedValue;
        }
        set
        {
            using var _ = _lock.EnterScope();
            if (value is not null)
            {
                this.value = value;
            }
        }
    }
}

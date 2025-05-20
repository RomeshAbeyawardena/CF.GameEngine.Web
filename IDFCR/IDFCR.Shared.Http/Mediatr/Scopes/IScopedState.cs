namespace IDFCR.Shared.Http.Mediatr.Scopes
{
    public interface IScopedState
    {
        string Key { get; }
        object? Value { get; }
        Type Type { get; }
    }

    public interface IScopedState<T> : IScopedState
    {
        new T? Value { get; }
    }

}

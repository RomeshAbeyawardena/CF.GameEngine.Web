namespace IDFCR.Shared.Http.Mediatr.Scopes
{
    public record ScopedState(string Key, object? Value, Type Type) : IScopedState;

    public record ScopeState<T> : ScopedState, IScopedState<T>
    {
        public static IScopedState<T>? From(IScopedState scopedState)
        {
            var targetType = typeof(T);
            if (scopedState.Type != targetType)
            {
                throw new InvalidOperationException($"Cannot convert {scopedState.Type} to {targetType}");
            }
            if (scopedState.Value is T val)
            {
                return new ScopeState<T>(scopedState.Key, val);
            }

            return null;
        }

        public ScopeState(string key, T value) : base(key, value, typeof(T))
        {
            Value = value;
        }

        public new T Value { get; }
    }
}

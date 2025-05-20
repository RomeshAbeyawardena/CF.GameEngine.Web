using IDFCR.Shared.Abstractions.Results;
using System.Collections.Concurrent;

namespace IDFCR.Shared.Http.Mediatr
{
    internal interface IScopedStateWriter
    {
        Task<IUnitResult> WriteAsync(string key, object value, CancellationToken cancellationToken);
    }

    internal interface IScopedStateReader
    {
        Task<IUnitResult<IScopedState>> ReadAsync(string key, CancellationToken cancellationToken);
        Task<IUnitResult<IScopedState<T>>> ReadAsync<T>(string key, CancellationToken cancellationToken);
    }

    public class ScopedStateFactory : IScopedStateReader, IScopedStateWriter
    {
        private readonly ConcurrentDictionary<string, IScopedState> state = [];

        public async Task<IUnitResult<IScopedState>> ReadAsync(string key, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            if(state.TryGetValue(key, out var scopedStateEntry))
            {
                return UnitResult.FromResult(scopedStateEntry);
            }

            return new UnitResult(new KeyNotFoundException($"Key '{key}' not found in scoped state.")).As<IScopedState>();
        }

        public async Task<IUnitResult<IScopedState<T>>> ReadAsync<T>(string key, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
            if (state.TryGetValue(key, out var scopedStateEntry))
            {
                var result = ScopeState<T>.From(scopedStateEntry);

                if (result is null)
                {
                    return new UnitResult(new InvalidOperationException($"Key '{key}' is not of type '{typeof(T)}'.")).As<IScopedState<T>>();
                }

                return UnitResult.FromResult(result);
            }

            return new UnitResult(new InvalidOperationException($"Key '{key}' is not of type '{typeof(T)}'.")).As<IScopedState<T>>();
        }

        public async Task<IUnitResult> WriteAsync(string key, object value, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;

            state.AddOrUpdate(key, new ScopedState(key, value, value.GetType()), (_, _) => new ScopedState(key, value, value.GetType()));

            return new UnitResult(IsSuccess: true);
        }
    }
}

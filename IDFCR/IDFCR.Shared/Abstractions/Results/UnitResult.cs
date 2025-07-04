﻿using IDFCR.Shared.Abstractions.Paging;
using IDFCR.Shared.Exceptions;
using System.Collections;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

namespace IDFCR.Shared.Abstractions.Results;

public record UnitResult(Exception? Exception = null, UnitAction Action = UnitAction.None,
    bool IsSuccess = false, FailureReason? FailureReason = null) : IUnitResult
{
    public static IUnitResult<T> NotFound<T>(object id, Exception? innerException = null, FailureReason? failureReason = Results.FailureReason.NotFound) 
        => Failed<T>(new EntityNotFoundException(typeof(T), id, innerException), UnitAction.None, failureReason);

    public static IUnitResult<T> Failed<T>(Exception exception, UnitAction action = UnitAction.None, FailureReason? FailureReason = null)
        => new UnitResult<T>(default, action, false, exception, FailureReason);

    public static IUnitResult<T> FromResult<T>(T? result, UnitAction action = UnitAction.Get,
        bool isSuccess = true, Exception? exception = null)
    {
        return new UnitResult<T>(result, action, isSuccess, exception);
    }

    internal readonly ConcurrentDictionary<string, object?> _metaProperties = [];
    public object? this[string key] { get => _metaProperties[key]; }

    public int Count => _metaProperties.Count;

    public IEnumerable<string> Keys => _metaProperties.Keys;
    public IEnumerable<object?> Values => _metaProperties.Values;

    public IUnitResult AddMeta(string key, object? value)
    {
        ArgumentException.ThrowIfNullOrEmpty(key, nameof(key));
        _metaProperties.AddOrUpdate(key, value, (_, _) => value);   
        return this;
    }

    public IUnitResult<T> As<T>() => new UnitResult<T>(default, Action, IsSuccess, Exception);

    public IUnitResultCollection<T> AsCollection<T>() => new UnitResultCollection<T>(default, Action, IsSuccess, Exception);

    public bool ContainsKey(string key)
    {
        return _metaProperties.ContainsKey(key);
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return _metaProperties.GetEnumerator();
    }

    public bool TryGetValue(string key, [MaybeNullWhen(false)] out object? value)
    {
        return _metaProperties.TryGetValue(key, out value);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public record UnitResult<TResult>(TResult? Result = default, UnitAction Action = UnitAction.None,
    bool IsSuccess = true, Exception? Exception = null, FailureReason? FailureReason = null)
    : UnitResult(Exception, Action, IsSuccess, FailureReason), IUnitResult<TResult>
{
    [MemberNotNullWhen(true, nameof(Result))]
    public bool HasValue => IsSuccess && Result is not null;
}

public interface IUnitPagedResult<TResult> : IUnitResult<IEnumerable<TResult>>
{
    int TotalRows { get; }
    IPagedQuery PagedQuery { get; }
}

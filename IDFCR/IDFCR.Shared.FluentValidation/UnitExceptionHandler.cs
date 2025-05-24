using FluentValidation;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.FluentValidation.Constants;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace IDFCR.Shared.FluentValidation;

public class UnitExceptionHandler<TRequest, TResponse, TException>(ILogger<UnitExceptionHandler<TRequest, TResponse, TException>> logger) : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : notnull
    where TException : Exception
{
    public static Type? UnitResultLookup(Type type)
    {
        var isGeneric = type.IsGenericType;

        if (type.Name.StartsWith("IUnitResultCollection"))
        {
            return typeof(UnitResultCollection<>);
        }

        if (type.Name.StartsWith("IUnitPagedResult"))
        {
            return typeof(UnitPagedResult<>);
        }

        if (type.Name.StartsWith("IUnitResult"))
        {
            return isGeneric ? typeof(UnitResult<>) : typeof(UnitResult);
        }

        return null;
    }

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        bool isHandled = exception is UnauthorizedAccessException || exception is ValidationException;
        var unitAction = UnitAction.None;
        if (isHandled && typeof(TResponse).IsAssignableTo(typeof(IUnitResult)))
        {
            Dictionary<string, object?> errors = [];

            if (exception is ValidationException validationException)
            {
                errors = validationException.Errors
                    .ToDictionary(x => x.PropertyName, x => (object?)x.ErrorMessage);

                var conflictErrorCode = validationException.Data[Errorcodes.Conflict];
                logger.LogInformation("Validation exception occurred with conflict code: {ConflictErrorCode}", conflictErrorCode);

                if (conflictErrorCode is not null && conflictErrorCode is bool isConflict && isConflict)
                {
                    logger.LogInformation("Unit action has been written");
                    unitAction = UnitAction.Conflict;
                }
            }

            var baseResponseType = typeof(TResponse);
            
            var mayBeGenericResponseType = baseResponseType.IsGenericType ? baseResponseType.GetGenericTypeDefinition() : baseResponseType;
            
            var implementationType = UnitResultLookup(mayBeGenericResponseType) ?? throw new NotSupportedException();

            var genericArguments = baseResponseType.IsGenericType 
                ? typeof(TResponse).GetGenericArguments()
                : [];


            if ((baseResponseType.IsGenericType
                ? Activator.CreateInstance(implementationType.MakeGenericType(genericArguments),
                null, unitAction, false, exception)
                : Activator.CreateInstance(implementationType, exception, unitAction, false)) is not IUnitResult response)
            {
                return Task.CompletedTask;
            }

            if (errors is not null)
            {
                foreach (var (k, v) in errors)
                {
                    response.AddMeta(k,v);
                }
            }

            state.SetHandled((TResponse)response!);
        }

        return Task.CompletedTask;
    }
}

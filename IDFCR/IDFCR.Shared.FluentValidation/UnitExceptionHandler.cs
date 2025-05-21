using FluentValidation;
using IDFCR.Shared.Abstractions.Results;
using MediatR.Pipeline;

namespace IDFCR.Shared.FluentValidation;

public class UnitExceptionHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
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

        if (isHandled && typeof(TResponse).IsAssignableTo(typeof(IUnitResult)))
        {
            var errors = (exception is ValidationException validationException) ? validationException.Errors
                    .ToDictionary(x => x.PropertyName, x => (object?)x.ErrorMessage) : [];
            
            var baseResponseType = typeof(TResponse);
            
            var mayBeGenericResponseType = baseResponseType.IsGenericType ? baseResponseType.GetGenericTypeDefinition() : baseResponseType;
            
            var implementationType = UnitResultLookup(mayBeGenericResponseType) ?? throw new NotSupportedException();

            var genericArguments = baseResponseType.IsGenericType 
                ? typeof(TResponse).GetGenericArguments()
                : [];


            if ((baseResponseType.IsGenericType
                ? Activator.CreateInstance(implementationType.MakeGenericType(genericArguments),
                null, UnitAction.None, false, exception)
                : Activator.CreateInstance(implementationType, exception, UnitAction.None, false)) is not IUnitResult response)
            {
                return Task.CompletedTask;
            }

            if (errors is not null)
            {
                foreach (var (k, v) in errors)
                {
                    if (string.IsNullOrWhiteSpace(k))
                    {
                        continue;
                    }
                    response.AddMeta(k,v);
                }
            }

            state.SetHandled((TResponse)response!);
        }

        return Task.CompletedTask;
    }
}

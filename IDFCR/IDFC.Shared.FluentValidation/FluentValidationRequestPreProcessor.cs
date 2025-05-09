using FluentValidation;
using FluentValidation.Results;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Http.Results;
using MediatR;
using MediatR.Pipeline;

namespace IDFC.Shared.FluentValidation;

public class FluentValidationRequestPreProcessor<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators) 
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull, IRequest<TResponse>
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        List<ValidationFailure> errors = [];
        var validationContext = new ValidationContext<TRequest>(request);
        foreach(var validator in validators)
        {
            var result = await validator.ValidateAsync(validationContext, cancellationToken);
            errors.AddRange(result.Errors);
        }

        if(errors.Count > 0)
        {
            throw new ValidationException(errors);
        }
    }
}

public class UnitExceptionHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : notnull, IRequest<TResponse>
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
        if(exception is ValidationException validationException && typeof(TResponse).IsAssignableTo(typeof(IUnitResult)))
        {
            var errors = validationException.Errors
                .ToDictionary(x => x.PropertyName, x => (object?)x.ErrorMessage);
            var baseResponseType = typeof(TResponse);
            
            var mayBeGenericResponseType = baseResponseType.IsGenericType ? baseResponseType.GetGenericTypeDefinition() : baseResponseType;
            
            var implementationType = UnitResultLookup(mayBeGenericResponseType) ?? throw new NotSupportedException();

            var genericArguments = baseResponseType.IsGenericType 
                ? typeof(TResponse).GetGenericArguments()
                : [];

            var response = (baseResponseType.IsGenericType
                ? Activator.CreateInstance(implementationType.MakeGenericType(genericArguments), 
                null, UnitAction.None, false, exception)
                : Activator.CreateInstance(implementationType, exception, UnitAction.None, false)) as IUnitResult;

            if(response == null)
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

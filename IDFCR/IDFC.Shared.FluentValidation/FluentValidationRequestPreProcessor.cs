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

public class ExceptionHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : notnull, IRequest<TResponse>
    where TException : Exception
{
    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        if(exception is ValidationException validationException && typeof(TResponse).IsAssignableFrom(typeof(IApiResult)))
        {
            var errors = validationException.Errors
                .ToDictionary(x => x.PropertyName, x => (object?)x.ErrorMessage);

            var response = Activator.CreateInstance(typeof(UnitResult), exception, UnitAction.None, false) as IApiResult;
            if(response == null)
            {
                return Task.CompletedTask;
            }

            if (errors is not null)
            {
                response?.AppendMeta(errors);
            }

            state.SetHandled((TResponse)response!);
        }

        return Task.CompletedTask;
    }
}

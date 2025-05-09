using FluentValidation;
using FluentValidation.Results;
using MediatR;
using MediatR.Pipeline;

namespace IDFC.Shared.FluentValidation;

internal class FluentValidationRequestPreProcessor<TRequest>(IEnumerable<IValidator<TRequest>> validators) 
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
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

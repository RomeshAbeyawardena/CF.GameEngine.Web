using FluentValidation;
using FluentValidation.Results;
using IDFCR.Shared.FluentValidation.Constants;
using MediatR.Pipeline;

namespace IDFCR.Shared.FluentValidation;

internal class FluentValidationRequestPreProcessor<TRequest>(IEnumerable<IValidator<TRequest>> validators) 
    : IRequestPreProcessor<TRequest>
    where TRequest : notnull
{
    public async Task Process(TRequest request, CancellationToken cancellationToken)
    {
        List<ValidationFailure> errors = [];
        var validationContext = new ValidationContext<TRequest>(request);
        bool isConflict = false;
        foreach(var validator in validators)
        {
            var result = await validator.ValidateAsync(validationContext, cancellationToken);
            isConflict = result.Errors.Any(x => x.ErrorCode == Errorcodes.Conflict);
            
            errors.AddRange(result.Errors);
        }

        if(errors.Count > 0)
        {
            var validationException = new ValidationException(errors);

            if(isConflict)
            {
                validationException.Data.Add(Errorcodes.Conflict, true);
            }

            throw validationException;
        }
    }
}

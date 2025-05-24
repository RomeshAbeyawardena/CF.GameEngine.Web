using FluentValidation;
using FluentValidation.Results;
using IDFCR.Shared.FluentValidation.Constants;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace IDFCR.Shared.FluentValidation;

internal class FluentValidationRequestPreProcessor<TRequest>(ILogger<FluentValidationRequestPreProcessor<TRequest>> logger,
    IEnumerable<IValidator<TRequest>> validators) 
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
            logger.LogInformation($"Is {(isConflict ? string.Empty : "not a ")}Conflict");
            errors.AddRange(result.Errors);
        }

        

        if (errors.Count > 0)
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

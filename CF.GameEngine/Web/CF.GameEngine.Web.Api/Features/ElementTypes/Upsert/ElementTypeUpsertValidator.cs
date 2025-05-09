using CF.GameEngine.Web.Api.Features.ElementTypes.Get;
using FluentValidation;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.ElementTypes.Upsert;

public class ElementTypeUpsertValidator : AbstractValidator<ElementTypeUpsertCommand>
{
    private readonly IMediator _mediator;
    public ElementTypeUpsertValidator(IMediator mediator)
    {
        _mediator = mediator;
        RuleFor(x => x.ElementType)
            .NotNull()
            .WithMessage("ElementType cannot be null.");
        RuleFor(x => x.ElementType.Name)
            .NotEmpty()
            .WithMessage("ElementType name cannot be empty.");
        RuleFor(x => x.ElementType.Key)
            .NotEmpty()
            .MustAsync(MustBeUnique)
            .WithMessage("ElementType key must be unique.");
    }

    private async Task<bool> MustBeUnique(string key, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ElementTypeFindQuery(Key: key), cancellationToken);
        return !result.HasValue;
    }
}

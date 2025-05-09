using FluentValidation;

namespace CF.GameEngine.Web.Api.Features.Element.Put;

public class PutElementValidator : AbstractValidator<PutElementCommand>
{
    public PutElementValidator()
    {
        RuleFor(x => x.Element.Id)
            .NotEqual(Guid.Empty)
            .WithMessage("Element ID must be provided for update.");
    }
}


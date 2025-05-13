using CF.GameEngine.Web.Api.Features.Element.Get;
using CF.GameEngine.Web.Api.Features.ElementTypes.Get;
using FluentValidation;
using IDFCR.Shared.Extensions;
using MediatR;

namespace CF.GameEngine.Web.Api.Features.Element.Upsert;

public class UpsertElementValidator : AbstractValidator<UpsertElementCommand>
{
    private readonly IMediator _mediator;

    public UpsertElementValidator(IMediator mediator)
    {
        _mediator = mediator;

        RuleFor(x => x.Element)
            .NotNull()
            .WithMessage("Element cannot be null.");

        RuleFor(x => x.Element.Name)
            .NotEmpty()
            .WithMessage("Element name cannot be empty.");

        RuleFor(x => x.Element.Key)
            .NotEmpty()
            .MustAsync(MustBeUnique)
            .WithMessage("Element key must be unique.");

        RuleFor(x => x)
            .MustAsync(HasValidElementType)
            .WithMessage("ElementTypeId or ElementType must refer to a valid element type.");

        RuleFor(x => x)
            .MustAsync(HasValidParentElement)
            .WithMessage("ParentElementId or ParentElement must refer to a valid parent element.");
    }

    private async Task<bool> MustBeUnique(string key, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ElementFindQuery(Key: key), cancellationToken);

        return !result.HasValue;
    }

    private async Task<bool> HasValidElementType(UpsertElementCommand command, CancellationToken cancellationToken)
    {
        if (command.Element.ElementTypeId.HasValue)
        {
            var result = await _mediator.Send(new ElementTypeFindByIdQuery(command.Element.ElementTypeId.Value), cancellationToken);
            return result.HasValue;
        }

        if (!string.IsNullOrWhiteSpace(command.Element.ElementType))
        {
            var result = await _mediator.Send(new ElementTypeFindQuery(Key: command.Element.ElementType), cancellationToken);
            var type = result.GetOneOrDefault();
            if (type is not null)
            {
                command.Element.ElementTypeId = type.Id; // update context
                return true;
            }
        }

        return false;
    }

    private async Task<bool> HasValidParentElement(UpsertElementCommand command, CancellationToken cancellationToken)
    {
        if (command.Element.ParentElementId.HasValue)
        {
            var result = await _mediator.Send(new ElementFindByIdQuery(command.Element.ParentElementId.Value), cancellationToken);
            return result.HasValue;
        }

        if (!string.IsNullOrWhiteSpace(command.Element.ParentElement))
        {
            var result = await _mediator.Send(new ElementFindQuery(Key: command.Element.ParentElement), cancellationToken);
            var element = result.GetOneOrDefault();
            if (element is not null)
            {
                command.Element.ParentElementId = element.Id; // update context
                return true;
            }
        }

        return true; // If no parent provided at all, it's valid
    }
}

using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Api.Features.Scopes.Post;
using FluentValidation;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.FluentValidation.Constants;
using MediatR;

namespace CF.Identity.Api.Features.Scopes.Upsert;

public class UpsertScopeValidator : AbstractValidator<UpsertScopeCommand>
{
    private readonly IMediator _mediator;
    public UpsertScopeValidator(IMediator mediator)
    {
        _mediator = mediator;

        RuleFor(x => x.Scope.Key)
            .NotEmpty().WithMessage("Scope key is required.")
            .MaximumLength(100).WithMessage("Scope key must not exceed 100 characters.");

        RuleFor(x => x.Scope.Name)
            .MaximumLength(100).WithMessage("Scope name must not exceed 100 characters.");

        RuleFor(x => x.Scope.ClientId)
            .NotNull()
            .When(x => string.IsNullOrWhiteSpace(x.Scope.Client))
            .MustAsync(ExistAsync)
            .WithName("ClientId_Exists");

        RuleFor(x => x.Scope.Client)
            .NotNull()
            .When(x => !x.Scope.ClientId.HasValue);

        RuleFor(x => x.Scope)
            .MustAsync(ExistAsync)
            .WithName("UniqueClient")
            .WithErrorCode(Errorcodes.Conflict)
            .When(x => !x.Scope.ClientId.HasValue);
    }

    private async Task<bool> ExistAsync(Guid? clientId, CancellationToken token)
    {
        if (!clientId.HasValue)
        {
            return true;
        }

        var client = (await _mediator.Send(new FindClientByIdQuery(clientId.Value), token)).GetResultOrDefault();

        return client is not null;
    }

    private async Task<bool> ExistAsync(EditableScopeDto scope, CancellationToken token)
    {
        if (string.IsNullOrWhiteSpace(scope.Client) || scope.ClientId.HasValue)
        {
            return true;
        }

        var client = (await _mediator
            .Send(new FindClientQuery(scope.Client), token)).GetOneOrDefault();

        if (client is null)
        {
            return false;
        }

        scope.ClientId = client.Id;
        return true;
    }
}

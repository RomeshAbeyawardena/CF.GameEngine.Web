using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Api.Features.User.Get;
using FluentValidation;
using IDFCR.Http.Authentication.Abstractions;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.FluentValidation.Constants;
using MediatR;

namespace CF.Identity.Api.Features.Clients.Post;

public class UpsertClientValidator : AbstractValidator<PostClientCommand>
{
    private readonly IMediator _mediator;
    private readonly IAuthenticatedUserContext _userContext;
    public UpsertClientValidator(IMediator mediator, IAuthenticatedUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
        RuleFor(x => x.Client.Name).NotEmpty()
            .WithMessage("Client name is required.")
            .MaximumLength(256)
            .WithMessage("Client name must not exceed 256 characters.");
        RuleFor(x => x.Client.DisplayName).MaximumLength(256);
        RuleFor(x => x.Client.ValidFrom).NotEmpty()
            .WithMessage("Valid from date is required.");
        RuleFor(x => x.Client.Reference).NotEmpty()
            .WithMessage("Client reference is required.")
            .MaximumLength(256)
            .WithMessage("Client reference must not exceed 256 characters.");

        RuleFor(x => x.Client).MustAsync(BeUnique)
            .WithName("Client_not_unique")
            .WithMessage("Client must have a unique name")
            .WithErrorCode(Errorcodes.Conflict);

        RuleFor(x => x.Client).MustAsync(EnsureNonSystemClientAuthenticatedUserCantAddSystemClients)
            .WithName("Non_System_Client_Cant_Add_System_Clients")
            .WithMessage("Non-system clients cannot add system clients.")
            .WithErrorCode(Errorcodes.Conflict);
    }

    public async Task<bool> BeUnique(EditableClientDto client, CancellationToken cancellationToken)
    {
        var result = (await _mediator.Send(new FindClientQuery(client.Reference), cancellationToken)).GetOneOrDefault();

        return result is null;
    }

    public async Task<bool> EnsureNonSystemClientAuthenticatedUserCantAddSystemClients(EditableClientDto client, CancellationToken cancellationToken)
    {
        await Task.CompletedTask;
        var authenticatedClient = _userContext.Client;

        if (authenticatedClient is null || _userContext.Client is null)
        {
            return false;
        }

        var authenticatedUser = (await _mediator
            .Send(new FindUserByIdQuery(authenticatedClient.UserId.GetValueOrDefault(), Bypass: true), cancellationToken)).GetResultOrDefault();

        if (authenticatedUser is null)
        {
            //fail-fast : We should not be here, whose been exposing secure entry points as free-for-all?
            return false;
        }

        if (!client.IsSystem)
        {
            return true;
        }

        return (client.IsSystem && authenticatedClient.IsSystem && authenticatedUser.IsSystem);
    }
}

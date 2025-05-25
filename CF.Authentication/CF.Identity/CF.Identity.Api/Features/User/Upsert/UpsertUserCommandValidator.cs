using CF.Identity.Api.Features.Clients;
using CF.Identity.Api.Features.Clients.Get;
using CF.Identity.Api.Features.User.Get;
using FluentValidation;
using IDFCR.Http.Authentication.Abstractions;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.FluentValidation.Constants;
using MediatR;

namespace CF.Identity.Api.Features.User.Upsert;

public class UpsertUserCommandValidator : AbstractValidator<UpsertUserCommand>
{
    private readonly IMediator _mediator;
    private readonly IAuthenticatedUserContext _userContext;
    private ClientDetailResponse? client;

    public UpsertUserCommandValidator(IMediator mediator, IAuthenticatedUserContext userContext)
    {
        _mediator = mediator;
        _userContext = userContext;
        RuleFor(x => x.User.Username).NotEmpty().WithMessage("Username is required.");
        RuleFor(x => x.User.EmailAddress).NotEmpty().WithMessage("Email address is required.");
        RuleFor(x => x.User.HashedPassword).NotEmpty().WithMessage("Password is required.");
        RuleFor(x => x.User.Firstname).NotEmpty().WithMessage("Firstname is required.");
        RuleFor(x => x.User.Lastname).NotEmpty().WithMessage("Lastname is required.");
        RuleFor(x => x.User.PrimaryTelephoneNumber).NotEmpty().WithMessage("Primary telephone number is required.");
        RuleFor(x => x).MustAsync(HaveValidClientAsync).WithName("Existing_Client")
            .WithMessage("Unable to find client by id or name required for user enrollment")
            .WithErrorCode(Errorcodes.Conflict);

        RuleFor(x => x).MustAsync(BeUnique).WithName("Unique_Username")
            .WithMessage("Username and email must be unique for a given client/realm")
            .WithErrorCode(Errorcodes.Conflict);
        RuleFor(x => x).MustAsync(EnsureNonSystemClientAuthenticatedUserCantAddUsersForSystemClients)
            .WithName("Non_System_Client_User_Cant__Add_System_Users")
            .WithMessage("System-client, system-user can only add system users.")
            .WithErrorCode(Errorcodes.Conflict);
    }

    public async Task<bool> EnsureNonSystemClientAuthenticatedUserCantAddUsersForSystemClients(UpsertUserCommand request, CancellationToken cancellationToken)
    {
        var authenticatedClient = _userContext.Client;

        if (authenticatedClient is null)
        {
            //fail-fast : We should not be here, whose been exposing secure entry points as free-for-all?
            return false;
        }

        var authenticatedUser = (await _mediator
            .Send(new FindUserByIdQuery(authenticatedClient.UserId.GetValueOrDefault(), Bypass: true), cancellationToken))
            .GetResultOrDefault();

        if (authenticatedUser is null)
        {
            //fail-fast : We should not be here, whose been exposing secure entry points as free-for-all?
            return false;
        }

        var isSystemClient = authenticatedClient?.IsSystem ?? false;

        if (client is null)
        {
            //fail-fast : The last validation failed so this will fail too as its a dependency!
            return false;
        }

        // User is not trying to add a user for a system client
        if (!client.IsSystem && !request.User.IsSystem)
        {
            return true;
        }

        // User is a system user belonging to a system client.
        return isSystemClient && client.IsSystem && authenticatedUser.IsSystem;
    }

    public async Task<bool> BeUnique(UpsertUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (user.ClientId == Guid.Empty)
        {
            return false;
        }

        var existingUser = (await _mediator.Send(new FindUsersQuery(user.ClientId, user.Username, Bypass: true), cancellationToken)).GetOneOrDefault();

        return existingUser is null;
    }

    public async Task<bool> HaveValidClientAsync(UpsertUserCommand request, CancellationToken cancellationToken)
    {
        var user = request.User;
        if (user.ClientId != Guid.Empty)
        {
            client = (await _mediator.Send(new FindClientByIdQuery(user.ClientId, Bypass: true), cancellationToken)).GetResultOrDefault();
            return client is not null;
        }

        if (string.IsNullOrWhiteSpace(user.Client))
        {
            return false;
        }

        client = (await _mediator.Send(new FindClientQuery(user.Client), cancellationToken)).GetOneOrDefault();

        if (client is null)
        {
            return false;
        }

        request.User.ClientId = client.Id;
        return true;
    }
}

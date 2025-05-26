using CF.Identity.Api.Features.Scopes.Get;
using CF.Identity.Infrastructure.Features.Scope;
using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Http.Authentication.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.User.Assign;

public class AssignUserScopesCommandHandler(IAuthenticatedUserContext authenticatedUserContext, 
    IMediator mediator, IUserRepository userRepository) : IUnitRequestHandler<AssignUserScopesCommand>
{
    public async Task<IUnitResult> Handle(AssignUserScopesCommand request, CancellationToken cancellationToken)
    {
        var client = authenticatedUserContext.Client;
        if(client is null || !client.UserId.HasValue)
        {
            return UnitResult.NotFound<ScopeDto>(request.ClientId);
        }

        //ensure scopes not belonging to the request user don't get added!

        bool includePrivilegedScopes = client.IsSystem;

        var scopes = (await mediator.Send(FindScopesQuery.Instance(client.Id, client.UserId, keys: request.Scopes, includePrivilegedScoped: includePrivilegedScopes), cancellationToken))
            .GetResultOrDefault();

        if(scopes is null)
        {
            return UnitResult.NotFound<ScopeDto>(request.Scopes);
        }

        var scopesToAdd = scopes.Where(x => request.Scopes.Any(y => y.Equals(x.Key, StringComparison.InvariantCultureIgnoreCase)))
            .Select(x => x.Id);

        if (!scopesToAdd.Any())
        {
            return UnitResult.NotFound<ScopeDto>("No valid scopes matched the request.");
        }

        return await userRepository.SynchroniseScopesAsync(request.UserId, scopesToAdd, cancellationToken);
    }
}
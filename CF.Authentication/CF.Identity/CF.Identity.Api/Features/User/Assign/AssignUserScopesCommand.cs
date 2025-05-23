using CF.Identity.Api.Features.Scopes;
using CF.Identity.Api.Features.Scopes.Get;
using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Http.Authentication;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Mediatr;
using MediatR;

namespace CF.Identity.Api.Features.User.Assign;

public record AssignUserScopesCommand(Guid ClientId, Guid UserId, IEnumerable<string> Scopes, bool Bypass = false) : IUnitRequest, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [Roles.ScopeRead, Roles.UserWrite];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}

public class AssignUserScopesCommandHandler(IHttpContextAccessor httpContextAccessor, IMediator mediator, IUserRepository userRepository) : IUnitRequestHandler<AssignUserScopesCommand>
{
    public async Task<IUnitResult> Handle(AssignUserScopesCommand request, CancellationToken cancellationToken)
    {
        var client = httpContextAccessor.HttpContext?.User.GetClient();
        if(client is null || !client.UserId.HasValue)
        {
            return UnitResult.NotFound<ScopeDto>(request.ClientId);
        }

        //ensure scopes not belonging to the request user don't get added!
        var scopes = (await mediator.Send(new FindScopeQuery(client.Id, client.UserId, Keys: request.Scopes), cancellationToken))
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
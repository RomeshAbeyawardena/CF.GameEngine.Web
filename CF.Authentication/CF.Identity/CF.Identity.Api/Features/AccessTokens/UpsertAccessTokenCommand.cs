using CF.Identity.Infrastructure.Features.AccessToken;
using IDFCR.Shared.Abstractions.Results;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.AccessTokens;

public record UpsertAccessTokenCommand(AccessTokenDto AccessToken, bool Bypass = false) 
    : RoleRequirementBase(() => RoleRegistrar.List<AccessTokenRoles>(RoleCategory.Write)), IUnitRequest<Guid>;

public class UpsertAccessTokenCommandHandler(IAccessTokenRepository accessTokenRepository) : IUnitRequestHandler<UpsertAccessTokenCommand, Guid>
{
    public async Task<IUnitResult<Guid>> Handle(UpsertAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await accessTokenRepository.UpsertAsync(request.AccessToken.Map<Infrastructure.Features.AccessToken.AccessTokenDto>(), cancellationToken);
        await accessTokenRepository.SaveChangesAsync(cancellationToken);
        return result;
    }
}
using CF.Identity.Infrastructure.Features.AccessToken;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Abstractions.Roles;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessTokens.Delete;

public record BulkExpireAccessTokenCommand(IEnumerable<Guid> AccessTokenIds, string? RevokeReason = null, string? RevokedBy = null, bool Bypass = false) : IUnitRequest, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => RoleRegistrar.List<AccessTokenRoles>(RoleCategory.Write);
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}

public class BulkExpireAccessTokenCommandHandler(IAccessTokenRepository accessTokenRepository) : IUnitRequestHandler<BulkExpireAccessTokenCommand>
{
    public async Task<IUnitResult> Handle(BulkExpireAccessTokenCommand request, CancellationToken cancellationToken)
    {
        var result = await accessTokenRepository.BulkExpireAsync(request.AccessTokenIds,
            request.RevokeReason, request.RevokedBy, cancellationToken);

        await accessTokenRepository.SaveChangesAsync(cancellationToken);

        return result;
    }
}

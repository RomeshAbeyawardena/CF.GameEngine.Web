using CF.Identity.Infrastructure.Features.AccessToken;
using IDFCR.Shared.Abstractions;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.AccessTokens.Delete;

public record BulkExpireAccessTokenCommand(IEnumerable<Guid> AccessTokenIds, string? RevokeReason = null, string? RevokedBy = null, bool Bypass = false) : 
    RoleRequirementBase(() => RoleRegistrar.List<AccessTokenRoles>(RoleCategory.Write)), IUnitRequest;
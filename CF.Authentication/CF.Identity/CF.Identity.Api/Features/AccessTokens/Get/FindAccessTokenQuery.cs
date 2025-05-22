using CF.Identity.Infrastructure.Features.AccessToken;
using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Mediatr;
namespace CF.Identity.Api.Features.AccessTokens.Get;

public record FindAccessTokenQuery(string? ReferenceToken = null, 
    Guid? ClientId = null, string? Type = null, Guid? UserId = null,
    DateTimeOffset? ValidFrom = null, DateTimeOffset? ValidTo = null, 
    bool ShowAll = false, bool NoTracking = true, bool Bypass = false)
    : IUnitRequestCollection<AccessTokenDto>, IAccessTokenFilter, IRoleRequirement
{
    IEnumerable<string> IRoleRequirement.Roles => [Roles.GlobalRead, Roles.AccessTokenRead];
    RoleRequirementType IRoleRequirement.RoleRequirementType => RoleRequirementType.Some;
}

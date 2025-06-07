using CF.Identity.Infrastructure.Features.AccessToken;
using IDFCR.Shared.Abstractions;
using RoleRegistrar = IDFCR.Shared.Abstractions.Roles.RoleRegistrar;
using IDFCR.Shared.Abstractions.Roles.Records;
using IDFCR.Shared.Mediatr;
namespace CF.Identity.Api.Features.AccessTokens.Get;

public record FindAccessTokenQuery(string? ReferenceToken = null,
    Guid? ClientId = null, string? Type = null, Guid? UserId = null,
    DateTimeOffset? ValidFrom = null, DateTimeOffset? ValidTo = null,
    bool ShowAll = false, bool NoTracking = true, bool Bypass = false)
    : RoleRequirementBase(() => RoleRegistrar.List<AccessTokenRoles>(RoleCategory.Read)), IUnitRequestCollection<AccessTokenDto>, IAccessTokenFilter;

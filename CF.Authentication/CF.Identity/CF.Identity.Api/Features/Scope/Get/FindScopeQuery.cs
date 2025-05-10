using CF.Identity.Infrastructure.Features.Scope;
using IDFCR.Shared.Mediatr;

namespace CF.Identity.Api.Features.Scope.Get;

public record FindScopeQuery(Guid? ClientId = null, string? Key = null, IEnumerable<string>? Keys = null, bool NoTracking = true) : IUnitRequestCollection<ScopeDto>, IScopeFilter;

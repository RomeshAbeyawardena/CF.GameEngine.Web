using Microsoft.AspNetCore.Authorization;
namespace IDFCR.Http.Authentication;

public record ScopeClaimPolicy(string? Scope = null, IEnumerable<string>? Scopes = null) : IAuthorizationRequirement;

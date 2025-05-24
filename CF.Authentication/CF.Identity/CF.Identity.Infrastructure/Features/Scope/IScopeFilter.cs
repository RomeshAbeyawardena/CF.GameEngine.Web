using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Scope;

public interface IScopeFilter : IFilter<IScopeFilter>
{
    bool IncludePrivilegedScoped { get; }
    IEnumerable<string>? Keys { get; }
    Guid? ClientId { get; }
    Guid? UserId { get; }
    string? Key { get; }
}

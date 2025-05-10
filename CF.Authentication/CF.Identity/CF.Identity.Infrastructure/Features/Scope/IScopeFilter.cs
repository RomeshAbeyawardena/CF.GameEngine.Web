using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Scope;

public interface IScopeFilter : IFilter<IScopeFilter>
{
    IEnumerable<string>? Keys { get; }
    Guid? ClientId { get; }
    string? Key { get; }
}

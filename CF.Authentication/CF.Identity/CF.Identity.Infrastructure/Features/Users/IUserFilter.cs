using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Filters;

namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserFilter : IFilter<IUserFilter>, IInjectableFilter
{
    bool? IsSystem { get; }
    Guid ClientId { get; }
    string? NameContains { get; }
}

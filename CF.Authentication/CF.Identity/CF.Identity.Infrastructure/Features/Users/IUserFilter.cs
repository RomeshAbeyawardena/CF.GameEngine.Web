using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserFilter : IFilter<IUserFilter>
{
    bool? IsSystem { get; }
    Guid? ClientId { get; }
    string? NameContains { get; }
}

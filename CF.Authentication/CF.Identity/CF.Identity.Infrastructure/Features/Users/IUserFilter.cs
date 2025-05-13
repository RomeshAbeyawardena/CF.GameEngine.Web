using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserFilter : IFilter<IUserFilter>
{
    Guid? ClientId { get; }
    string? NameContains { get; }
}

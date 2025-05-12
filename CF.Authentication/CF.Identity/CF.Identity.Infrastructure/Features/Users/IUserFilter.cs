namespace CF.Identity.Infrastructure.Features.Users;

public interface IUserFilter
{
    Guid? ClientId { get; }
    string? NameContains { get; }
}

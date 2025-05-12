using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Users;

public interface IUser : IEditableUser
{
}

public interface IEditableUser : IUserSummary
{
    string EmailAddress { get; }
    string HashedPassword { get; }
}

public interface IUserSummary : IMappable<IUser>, IIdentifer
{
    Guid ClientId { get; }
    string Username { get; }
    string? PreferredUsername { get; }
    string Firstname { get; }
    string? MiddleName { get; }
    string? LastName { get; }
}

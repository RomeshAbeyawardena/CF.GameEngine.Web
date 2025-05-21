using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.Features.Users;

public interface IUser : IEditableUser
{
}

public interface IEditableUser : IUserDetail, IPIIRowVersion
{
    string EmailAddress { get; }
    string HashedPassword { get; }
}

public interface IUserSummary : IMappable<IUser>, IIdentifer
{
    bool IsSystem { get; }
    Guid ClientId { get; }
    string? PreferredUsername { get; }
}

public interface IUserDetail : IUserSummary
{
    string? Metadata { get; set; }
    string Username { get; }
    string Firstname { get; }
    string? Middlename { get; }
    string Lastname { get; }
    string PrimaryTelephoneNumber { get; }
}
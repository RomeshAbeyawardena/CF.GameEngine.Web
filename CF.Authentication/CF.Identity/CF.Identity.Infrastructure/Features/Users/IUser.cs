using IDFCR.Shared.Abstractions;
using IDFCR.Shared.Abstractions.Cryptography;

namespace CF.Identity.Infrastructure.Features.Users;

public interface IUser : IEditableUser, IPIIRowVersion
{
    DateTimeOffset? AnonymisedTimestamp { get; }
    string? Metadata { get; set; }
}

public interface IEditableUser : IUserDetail
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
    string Username { get; }
    string Firstname { get; }
    string? Middlename { get; }
    string Lastname { get; }
    string PrimaryTelephoneNumber { get; }
}
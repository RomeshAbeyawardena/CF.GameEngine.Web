using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Api.Features.User;

public class UserDto : MappableBase<IUser>, IUser
{
    protected override IUser Source => this;

    internal string FormatName()
    {
        var name = Firstname;
        if (!string.IsNullOrWhiteSpace(Middlename))
        {
            name += $" {Middlename}";
        }

        name += $" {Lastname}";

        return name;
    }

    public string EmailAddress { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    public Guid ClientId { get; set; }
    public string Username { get; set; } = null!;
    public string? PreferredUsername { get; set; }
    public string Firstname { get; set; } = null!;
    public string? Middlename { get; set; }
    public string Lastname { get; set; } = null!;
    public Guid Id { get; set; }
    public bool IsSystem { get; set; }
    public string RowVersion { get; set; } = null!;
    public string? Metadata { get; set; }

    public override void Map(IUser source)
    {
        EmailAddress = source.EmailAddress;
        HashedPassword = source.HashedPassword;
        ClientId = source.ClientId;
        Username = source.Username;
        PreferredUsername = source.PreferredUsername;
        Firstname = source.Firstname;
        Middlename = source.Middlename;
        Lastname = source.Lastname;
        Id = source.Id;
        IsSystem = source.IsSystem;
        RowVersion = source.RowVersion;
        Metadata = source.Metadata;
    }
}

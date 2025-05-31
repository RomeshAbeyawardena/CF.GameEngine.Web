using CF.Identity.Infrastructure.Features.Users;
using IDFCR.Shared.Abstractions;

namespace CF.Identity.Infrastructure.SqlServer.Models;

public class DbUser : MappableBase<IUser>, IUser
{
    protected override IUser Source => this;
    string IUserDetail.Firstname => FirstCommonName.Value;
    string? IUserDetail.Middlename => MiddleCommonName?.Value;
    string IUserDetail.Lastname => LastCommonName.Value;

    public string EmailAddress { get; set; } = null!;
    public string EmailAddressHmac { get; set; } = null!;
    public string EmailAddressCI { get; set; } = null!;
    public string HashedPassword { get; set; } = null!;
    
    public string Username { get; set; } = null!;
    public string UsernameHmac { get; set; } = null!;
    public string UsernameCI { get; set; } = null!;

    public Guid FirstCommonNameId { get; set; }
    public Guid? MiddleCommonNameId { get; set; }
    public Guid LastCommonNameId { get; set; }

    public Guid ClientId { get; set; }
    public string? PreferredUsername { get; set; }
    public string PreferredUsernameHmac { get; set; } = null!;
    public string? PreferredUsernameCI { get; set; } = null!;

    public Guid Id { get; set; }
    public bool IsSystem { get; set; }
    public string RowVersion { get; set; } = null!;
    
    public string PrimaryTelephoneNumber { get; set; } = null!;
    public string PrimaryTelephoneNumberHmac { get; set; } = null!;

    public virtual DbClient Client { get; set; } = null!;
    public virtual DbCommonName FirstCommonName { get; set; } = null!;
    public virtual DbCommonName MiddleCommonName { get; set; } = null!;
    public virtual DbCommonName LastCommonName { get; set; } = null!;

    public virtual ICollection<DbUserScope> UserScopes { get; set; } = [];

    public string? Metadata { get; set; }
    public DateTimeOffset? AnonymisedTimestamp { get; set; }

    public override void Map(IUser source)
    {
        EmailAddress = source.EmailAddress;
        Username = source.Username;
        HashedPassword = source.HashedPassword;
        ClientId = source.ClientId;
        PreferredUsername = source.PreferredUsername;
        Id = source.Id;
        IsSystem = source.IsSystem;
        RowVersion = source.RowVersion;
        Metadata = source.Metadata;
        PrimaryTelephoneNumber = source.PrimaryTelephoneNumber;
        AnonymisedTimestamp = source.AnonymisedTimestamp;
    }
}

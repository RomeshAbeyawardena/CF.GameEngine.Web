
using IDFCR.Shared.Abstractions.Roles;

namespace IDFCR.Shared.Abstractions;

[Flags]
public enum RoleCategory
{
    None = 0,
    Read = 1,
    Write = 2,
    Delete = 4,
    Execute = 8,
}

public record DefaultRoleDescriptor(string Key, bool IsPrivileged = false) : IRoleDescriptor
{
    public string? DisplayName { get; init; } 
    public string? Description { get; init; }
    public string? Prefix { get; init; } = null;
    public RoleCategory Category { get; init; }
}

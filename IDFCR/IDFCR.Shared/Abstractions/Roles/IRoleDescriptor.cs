namespace IDFCR.Shared.Abstractions.Roles;

public interface IRoleDescriptor
{
    RoleCategory Category { get; }
    string? DisplayName { get; }
    string? Description { get; }
    string? Prefix { get; }
    string Key { get; }
    bool IsPrivileged { get; }
}

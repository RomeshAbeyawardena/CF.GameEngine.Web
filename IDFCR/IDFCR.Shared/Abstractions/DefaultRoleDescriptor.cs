
namespace IDFCR.Shared.Abstractions;

public record DefaultRoleDescriptor(string Key, bool IsPrivileged = false) : IRoleDescriptor
{
    public string? DisplayName { get; init; } 
    public string? Description { get; init; }
    public string? Prefix { get; init; } = null;
}

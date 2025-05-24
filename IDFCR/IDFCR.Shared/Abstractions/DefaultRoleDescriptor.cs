
namespace IDFCR.Shared.Abstractions;

public record DefaultRoleDescriptor(string Key, bool IsPrivileged = false) : IRoleDescriptor
{
    public string? Prefix { get; init; } = null;
}

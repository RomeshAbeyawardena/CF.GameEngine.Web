
namespace IDFCR.Shared.Abstractions;

public interface IRoleDescriptor
{
    string? DisplayName { get; }
    string? Description { get; }
    string? Prefix { get; }
    string Key { get; }
    bool IsPrivileged { get; }
}

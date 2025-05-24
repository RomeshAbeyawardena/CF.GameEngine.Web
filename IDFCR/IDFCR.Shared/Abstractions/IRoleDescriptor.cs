
namespace IDFCR.Shared.Abstractions;

public interface IRoleDescriptor
{
    string? Prefix { get; }
    string Key { get; }
    bool IsPrivileged { get; }
}

namespace IDFCR.Shared.Abstractions;

public interface IRoleRegistrarCollector
{
    IReadOnlyDictionary<string, List<IRoleDescriptor>> GroupedByPrefix { get; }
    IEnumerable<IRoleDescriptor> Roles { get; }
    public IEnumerable<string> RoleNames { get; }
}

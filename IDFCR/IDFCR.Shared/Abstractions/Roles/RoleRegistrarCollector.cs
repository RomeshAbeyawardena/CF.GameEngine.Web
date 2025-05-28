namespace IDFCR.Shared.Abstractions.Roles;

public class RoleRegistrarCollector(IEnumerable<IRoleRegistrar> roleRegistrars) : IRoleRegistrarCollector
{
    public IEnumerable<IRoleDescriptor> Roles => [.. roleRegistrars.SelectMany(x => x)];
    public IEnumerable<string> RoleNames => Roles.Select(x => x.Key);
    public IReadOnlyDictionary<string, List<IRoleDescriptor>> GroupedByPrefix =>
    roleRegistrars
        .Where(r => !string.IsNullOrWhiteSpace(r.Prefix))
        .GroupBy(r => r.Prefix!)
        .ToDictionary(
            g => g.Key,
            g => g.SelectMany(r => r).ToList());
}

namespace IDFCR.Shared.Abstractions;

public class RoleRegistrarCollector(IEnumerable<IRoleRegistrar> roleRegistrars) : IRoleRegistrarCollector
{
    public IEnumerable<IRoleDescriptor> Roles => [.. roleRegistrars.SelectMany(x => x)];
}

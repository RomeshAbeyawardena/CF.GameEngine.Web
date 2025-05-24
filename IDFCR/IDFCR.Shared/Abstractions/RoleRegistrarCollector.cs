namespace IDFCR.Shared.Abstractions;

public class RoleRegistrarCollector(IEnumerable<IRoleRegistrar> roleRegistrars) : IRoleRegistrarCollector
{
    public IEnumerable<string> Roles => [.. roleRegistrars.SelectMany(x => x)];
}

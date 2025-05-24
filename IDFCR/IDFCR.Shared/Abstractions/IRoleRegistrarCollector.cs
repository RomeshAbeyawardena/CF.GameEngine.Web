namespace IDFCR.Shared.Abstractions;

public interface IRoleRegistrarCollector
{
    IEnumerable<IRoleDescriptor> Roles { get; }
}

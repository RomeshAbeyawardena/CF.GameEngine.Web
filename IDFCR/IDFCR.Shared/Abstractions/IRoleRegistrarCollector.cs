namespace IDFCR.Shared.Abstractions;

public interface IRoleRegistrarCollector
{
    IEnumerable<string> Roles { get; }
}

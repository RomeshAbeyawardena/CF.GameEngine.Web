namespace IDFCR.Http.Authentication.Abstractions;

public interface IRoleRegistrarCollector
{
    IEnumerable<string> Roles { get; }
}

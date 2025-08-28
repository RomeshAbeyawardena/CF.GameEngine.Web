namespace IDFCR.Shared.Abstractions;

public interface ILink<T>
    where T : IIdentifer, INamed
{
    Guid Id { get; }
    string Name { get; }
    T? Model { get; }
}

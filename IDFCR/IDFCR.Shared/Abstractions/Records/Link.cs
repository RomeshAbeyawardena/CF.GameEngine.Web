namespace IDFCR.Shared.Abstractions.Records;

public record Link<T>(Guid Id, string Name, T? Model) : ILink<T>
    where T : IIdentifer, INamed
{
    public Link(T model, Func<T, string> getName)
        : this(model.Id, getName(model), model)
    {
        
    }
}

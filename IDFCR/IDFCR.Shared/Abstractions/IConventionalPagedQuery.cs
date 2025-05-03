namespace IDFCR.Shared.Abstractions;

public interface IConventionalPagedQuery : IMappable<IPagedQuery>
{
    int? Take { get; }
    int? Skip { get; }
}

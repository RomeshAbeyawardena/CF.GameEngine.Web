namespace IDFCR.Shared.Abstractions;

public interface IPagedQuery : IMappable<IConventionalPagedQuery>
{
    int? PageSize { get; }
    int? PageIndex { get; }
}

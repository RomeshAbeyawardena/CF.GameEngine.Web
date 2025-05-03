namespace IDFCR.Shared.Abstractions.Paging;

public interface IPagedQuery : ISingularMappable<IPagedQuery>, ISingularMappable<IConventionalPagedQuery>
{
    int? PageSize { get; }
    int? PageIndex { get; }
}

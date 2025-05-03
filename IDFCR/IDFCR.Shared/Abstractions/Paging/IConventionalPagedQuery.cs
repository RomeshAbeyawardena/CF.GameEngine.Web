namespace IDFCR.Shared.Abstractions.Paging;

public interface IConventionalPagedQuery : ISingularMappable<IConventionalPagedQuery>, ISingularMappable<IPagedQuery>
{
    int? Take { get; }
    int? Skip { get; }
}

namespace IDFCR.Shared.Abstractions.Paging;

public class ConventionalPagedQuery() : IConventionalPagedQuery
{
    public ConventionalPagedQuery(IPagedQuery query) : this()
    {
        Map(query);
    }

    public int? Take { get; set; }
    public int? Skip { get; set; }

    public void Map(IConventionalPagedQuery source)
    {
        Take = source.Take;
        Skip = source.Skip;
    }

    public void Map(IPagedQuery source)
    {
        Take = source.PageSize;
        Skip = source.PageIndex.HasValue && source.PageSize.HasValue
            ? source.PageIndex * source.PageSize
            : null;
    }
}

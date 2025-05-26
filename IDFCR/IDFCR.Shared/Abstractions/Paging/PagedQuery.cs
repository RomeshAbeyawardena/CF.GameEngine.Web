namespace IDFCR.Shared.Abstractions.Paging;

public abstract record MappablePagedQuery<T> : Records.MappableBase<T>, IPagedQuery
{
    private readonly Lazy<IPagedQuery> _calculatedPagedQuery;
    private IPagedQuery PagedQuery => _calculatedPagedQuery.Value;
    protected MappablePagedQuery()
    {
        _calculatedPagedQuery = new(() => new PagedQuery(PageSize, PageIndex));
    }

    public int? PageSize { get; set; }
    public int? PageIndex { get; set;}

    public void Map(IPagedQuery source)
    {
        PagedQuery.Map(source);
    }

    public void Map(IConventionalPagedQuery source)
    {
        PagedQuery.Map(source);
    }
}

public record PagedQuery() : IPagedQuery
{
    public PagedQuery(int? pageSize, int? pageIndex) : this()
    {
        PageSize = pageSize;
        PageIndex = pageIndex;
    }

    public int? PageSize { get; set; }
    public int? PageIndex { get; set; }

    public void Map(IPagedQuery source)
    {
        PageIndex = source.PageIndex;
        PageSize = source.PageSize;
    }

    public void Map(IConventionalPagedQuery source)
    {
        if(source.Skip.HasValue && source.Take.HasValue)
        {
            PageSize = source.Take;
            PageIndex = source.Skip.HasValue && source.Take.HasValue && source.Take.Value != 0
                            ? source.Skip / source.Take
                            : null;
        }
    }
}

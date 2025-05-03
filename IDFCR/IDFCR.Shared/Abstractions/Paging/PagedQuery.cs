namespace IDFCR.Shared.Abstractions.Paging;

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
            PageIndex = source.Skip / source.Take;
        }
    }
}

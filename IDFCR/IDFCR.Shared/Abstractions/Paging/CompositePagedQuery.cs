namespace IDFCR.Shared.Abstractions.Paging;

public class CompositePagedQuery : ICompositePagedQuery
{
    public int? PageSize { get; set; }
    public int? PageIndex { get; set; }
    public int? Take { get; set; }
    public int? Skip { get; set; }

    public void Map(IConventionalPagedQuery source)
    {
        PageSize = source.Take;
        PageIndex = source.Skip.HasValue && source.Take.HasValue && source.Take.Value != 0
                        ? source.Skip / source.Take : null;
        Take = source.Take;
        Skip = source.Skip;
    }

    public void Map(IPagedQuery source)
    {
        PageSize = source.PageSize;
        PageIndex = source.PageIndex;
        Take = source.PageSize;
        Skip = source.PageIndex.HasValue && source.PageSize.HasValue
            ? source.PageIndex * source.PageSize
            : null;
    }
}
namespace IDFCR.Shared.Abstractions.Paging;

public class PageQuery : IPagedQuery
{
    public int? PageSize { get; set; }
    public int? PageIndex { get; set; }

    public void Map(IPagedQuery source)
    {
        throw new NotImplementedException();
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

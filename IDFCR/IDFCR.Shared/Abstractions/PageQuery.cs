namespace IDFCR.Shared.Abstractions;

public class PageQuery : MappableBase<IConventionalPagedQuery>, IPagedQuery
{
    public int? PageSize { get; set; }
    public int? PageIndex { get; set; }

    public override void Map(IConventionalPagedQuery source)
    {
        if (source.Skip.HasValue && source.Take.HasValue)
        {
            PageSize = source.Take;
            PageIndex = source.Skip / source.Take;
        }
    }

    protected override IConventionalPagedQuery Source =>
        new ConventionalPagedQuery
        {
            Skip = PageIndex.HasValue && PageSize.HasValue ? PageIndex * PageSize : null,
            Take = PageSize
        };
}

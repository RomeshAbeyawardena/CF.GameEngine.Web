namespace IDFCR.Shared.Abstractions;

public class ConventionalPagedQuery : MappableBase<IPagedQuery>, IConventionalPagedQuery
{
    protected override IPagedQuery Source => new PageQuery
    {
        PageSize = Take,
        PageIndex = Take.HasValue && Skip.HasValue && Take != 0
                ? Skip / Take
                : null
    };

    public int? Take { get; set; }
    public int? Skip { get; set; }

    public override void Map(IPagedQuery source)
    {
        Take = source.PageSize;
        Skip = source.PageIndex.HasValue && source.PageSize.HasValue
            ? source.PageIndex * source.PageSize
            : null;
    }
}

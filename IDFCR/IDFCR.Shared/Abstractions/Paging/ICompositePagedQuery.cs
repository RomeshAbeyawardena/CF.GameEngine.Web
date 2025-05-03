namespace IDFCR.Shared.Abstractions.Paging;

public interface ICompositePagedQuery : IPagedQuery, IConventionalPagedQuery
{

}

public class CompositePagedQuery : ICompositePagedQuery
{
    public int? PageSize { get; set; }
    public int? PageIndex { get; set; }
    public int? Take { get; set; }
    public int? Skip { get; set; }

    public void Map(IConventionalPagedQuery source)
    {
        throw new NotImplementedException();
    }

    public void Map(IPagedQuery source)
    {
        throw new NotImplementedException();
    }
}
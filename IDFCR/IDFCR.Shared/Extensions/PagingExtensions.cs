using IDFCR.Shared.Abstractions.Paging;

namespace IDFCR.Shared.Extensions;

public static class PagingExtensions
{
    public static PagedQuery ToPageQuery(this IConventionalPagedQuery source)
    {
        var result = new PagedQuery();
        result.Map(source);
        return result;
    }

    public static ConventionalPagedQuery ToConventional(this IPagedQuery source)
    {
        var result = new ConventionalPagedQuery();
        result.Map(source);
        return result;
    }
}


using IDFCR.Shared.Abstractions.Paging;

namespace IDFCR.Shared.Extensions;

public static class PagingExtensions
{
    public static PageQuery ToPageQuery(this IConventionalPagedQuery source)
    {
        var result = new PageQuery();
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


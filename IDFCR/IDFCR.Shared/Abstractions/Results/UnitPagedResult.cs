using IDFCR.Shared.Abstractions.Paging;

namespace IDFCR.Shared.Abstractions.Results;

public record UnitPagedResult<TResult> : UnitResult<IEnumerable<TResult>>, IUnitPagedResult<TResult>
{
    public IPagedQuery PagedQuery { get; }
    public int TotalRows { get; }
    public UnitPagedResult(IEnumerable<TResult>? result, int totalRows, IPagedQuery pagedQuery, UnitAction action = UnitAction.None,
    bool isSuccess = true, Exception? exception = null) : base(result, action, isSuccess, exception)
    {
        PagedQuery = pagedQuery;
        if (pagedQuery is not null)
        {
            base.AddMeta("pageIndex", pagedQuery.PageIndex);
            base.AddMeta("pageSize", pagedQuery.PageSize);
            TotalRows = totalRows;
            base.AddMeta("totalRows", totalRows);
            if (pagedQuery.PageSize.HasValue)
            {
                base.AddMeta("totalPages", (int)Math.Ceiling((double)totalRows / pagedQuery.PageSize.Value));
            }
        }
    }

    public UnitPagedResult(IEnumerable<TResult>? result, UnitAction action = UnitAction.None, bool isSuccess = true, Exception? exception = null) : this(result, 0, null!, action, isSuccess, exception)
    {

    }
}
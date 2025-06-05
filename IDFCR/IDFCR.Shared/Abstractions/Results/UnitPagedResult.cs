using IDFCR.Shared.Abstractions.Paging;

namespace IDFCR.Shared.Abstractions.Results;

public static class UnitPagedResult
{
    public static UnitPagedResult<TResult> FromResult<TResult>(
        IEnumerable<TResult>? result, 
        int totalRows, 
        IPagedQuery pagedQuery, 
        UnitAction action = UnitAction.None, 
        bool isSuccess = true, 
        Exception? exception = null, 
        FailureReason? failureReason = null)
    {
        return new UnitPagedResult<TResult>(result, totalRows, pagedQuery, action, isSuccess, exception, failureReason);
    }
}

public sealed record UnitPagedResult<TResult> : UnitResultCollection<TResult>, IUnitPagedResult<TResult>
{
    public IPagedQuery PagedQuery { get; }
    public int TotalRows { get; }
    public UnitPagedResult(
        IEnumerable<TResult>? result, 
        int totalRows, 
        IPagedQuery pagedQuery, 
        UnitAction action = UnitAction.None,
        bool isSuccess = true, 
        Exception? exception = null, 
        FailureReason? failureReason = null) 
        : base(result, action, isSuccess, exception, failureReason)
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
}
using IDFCR.Shared.Abstractions.Results;
using IDFCR.Shared.Extensions;
using IDFCR.Shared.Http.Mediatr;

namespace IDFCR.Shared.Http.Extensions;

public static class ScopeConstants
{
    public const string ScopedBypassKey = "scope:bypass";
}

public static class ScopeReaderExtensions
{
    public static async Task<bool> IsScopedBypassInvokedAsync(this IScopedStateReader reader, CancellationToken cancellationToken)
    {
        var result = (await reader.ReadAsync<bool>(ScopeConstants.ScopedBypassKey, cancellationToken)).GetResultOrDefault();
        return result?.Value ?? false;
    }

    public static Task<IUnitResult> SetScopedBypassAsync(this IScopedStateWriter writer, bool value, CancellationToken cancellationToken)
    {
        return writer.WriteAsync(ScopeConstants.ScopedBypassKey, value, cancellationToken);
    }

}

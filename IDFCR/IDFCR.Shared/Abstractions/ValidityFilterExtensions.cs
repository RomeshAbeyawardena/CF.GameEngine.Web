namespace IDFCR.Shared.Abstractions;

public record DateTimeOffsetRange(DateTimeOffset FromValue, DateTimeOffset? ToValue)
{
    public static DateTimeOffsetRange GetValidatyDateRange(DateTimeOffset fromValue, DateTimeOffset? toValue = null)
    {
        return new(fromValue.Date.AddDays(1).AddTicks(-1), toValue.GetValueOrDefault(fromValue));
    }
}

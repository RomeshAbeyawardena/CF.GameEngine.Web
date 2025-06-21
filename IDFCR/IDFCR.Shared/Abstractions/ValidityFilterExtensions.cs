namespace IDFCR.Shared.Abstractions;

public record DateTimeOffsetRange(DateTimeOffset FromValue, DateTimeOffset? ToValue)
{
    public static DateTimeOffsetRange GetValidatyDateRange(DateTimeOffset fromValue, DateTimeOffset? toValue = null)
    {
        return new(fromValue.Date.AddDays(1).AddTicks(-1), toValue.GetValueOrDefault(fromValue));
    }

    public static DateTimeOffsetRange FromStringArray(IEnumerable<string?> dateRange, DateTimeOffset defaultDate)
    {
        if(dateRange.Count() > 2)
        {
            throw new ArgumentOutOfRangeException(nameof(dateRange), "Expected a maximum of two date ranges");
        }

        DateTimeOffset from = defaultDate.Date,
            to = defaultDate.Date.AddDays(1).AddMicroseconds(-1);

        var fromDate = dateRange.FirstOrDefault() ?? throw new NullReferenceException("From date is missing");
        var toDate = dateRange.ElementAtOrDefault(1) ?? throw new NullReferenceException("To date is missing");

        bool hasFromDate = false;
        if (!string.IsNullOrWhiteSpace(fromDate) && DateTimeOffset.TryParse(fromDate, out var date))
        {
            from = date;
            hasFromDate = true;
        }

        if (!string.IsNullOrWhiteSpace(toDate) && DateTimeOffset.TryParse(toDate, out date))
        {
            to = date;
        }
        else if (hasFromDate)
        {
            to = from.AddDays(1).AddMicroseconds(-1);
        }

        return new DateTimeOffsetRange(from, to);
    }

    public static (string?, bool) TryFromStringArray(IEnumerable<string?> dateRange, DateTimeOffset defaultDate, out DateTimeOffsetRange range)
    {
        range = GetValidatyDateRange(defaultDate);
        try
        {
            range = FromStringArray(dateRange, defaultDate);
            return (string.Empty, true);
        }
        catch(Exception exception)
        {
            return (exception.Message, false);
        }
    }
}

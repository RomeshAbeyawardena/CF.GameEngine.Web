namespace IDFCR.Shared.Abstractions;

public interface IAuditModifiedTimestamp<T> : IAudit
    where T : struct
{
    T? ModifiedTimestampUtc { get; set; }
}

public interface IAuditModifiedTimestamp : IAuditModifiedTimestamp<DateTimeOffset>;
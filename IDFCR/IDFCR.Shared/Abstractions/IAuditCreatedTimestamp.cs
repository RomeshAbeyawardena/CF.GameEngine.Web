namespace IDFCR.Shared.Abstractions;

public interface IAuditCreatedTimestamp<T> : IAudit
    where T : struct
{
    T CreatedTimestampUtc { get; set; }
}

public interface IAuditCreatedTimestamp : IAuditCreatedTimestamp<DateTimeOffset>;

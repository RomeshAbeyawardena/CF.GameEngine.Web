using System.Text;

namespace IDFCR.Utility.Shared.Extensions;

public static class MigrationResultExtensions
{
    public static string ToSummary(this MigrationResult result)
    {
        var statusIcon = result.Status switch
        {
            MigrationStatus.Completed => "✓",
            MigrationStatus.CompletedWithErrors => "⚠",
            MigrationStatus.Failed => "✗",
            MigrationStatus.InProgress => "…",
            _ => "?"
        };

        var messageBuilder = new StringBuilder($"{statusIcon} [{result.Key}] - {result.Status}");

        if (!string.IsNullOrWhiteSpace(result.Message))
        {
            messageBuilder.Append($": {result.Message}");
        }
        else if (result.Exception is not null)
        {
            messageBuilder.Append($": Exception thrown ({result.Exception.GetType().Name})");
        }

        return messageBuilder.ToString();
    }

    public static string ToReport(this IEnumerable<MigrationResult> results)
    {
        var sb = new StringBuilder();
        foreach (var result in results)
        {
            sb.AppendLine(result.ToSummary());
        }
        return sb.ToString();
    }
}

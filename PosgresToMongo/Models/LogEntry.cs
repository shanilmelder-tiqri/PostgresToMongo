namespace PostgresToMongo.Models;

public class LogEntry
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
    public string? ServiceName { get; set; }
    public string? Message { get; set; }
    public LogLevel LogLevel { get; set; } = LogLevel.Info;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? CorrelationId { get; set; }
    public string? TraceId { get; set; }
    public string? SpanId { get; set; }
    public string? UserId { get; set; }
    public string? Environment { get; set; }
    public string? Version { get; set; }
    public Dictionary<string, object>? Metadata { get; set; }
    public Exception? Exception { get; set; }
    public string? Source { get; set; }
    public string? Category { get; set; }
}

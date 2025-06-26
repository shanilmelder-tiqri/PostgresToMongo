namespace PostgresToMongo.Data
{
    public class LogEntrySeedDto
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string ServiceName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public string LogLevel { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public string? CorrelationId { get; set; }
        public string? TraceId { get; set; }
        public string? SpanId { get; set; }
        public string? UserId { get; set; }
        public string? Environment { get; set; }
        public string? Version { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
        public string? Exception { get; set; }
        public string? Source { get; set; }
        public string? Category { get; set; }
    }
}

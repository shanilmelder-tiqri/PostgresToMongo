using Bogus;
using Microsoft.Extensions.Logging;
using PostgresToMongo.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;

namespace PostgresToMongo.Data
{
    public static class SeedData
    {
        public static async Task SeedDataAsync(PostgresDbContext context)
        {
            if (await context.LogEntries.AnyAsync())
                return;

            // Load log entries from JSON file
            var filePath = Path.Combine(AppContext.BaseDirectory, "Data", "SeedLogEntries.json");
            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Seed data file not found: {filePath}");

            var json = await File.ReadAllTextAsync(filePath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var fileResult = JsonSerializer.Deserialize<List<LogEntrySeedDto>>(json, options) ?? new List<LogEntrySeedDto>();
            foreach (var dto in fileResult)
            {
                var log = new LogEntry
                {
                    Id = dto.Id,
                    ServiceName = dto.ServiceName,
                    Message = dto.Message,
                    LogLevel = Enum.TryParse<Models.LogLevel>(dto.LogLevel, true, out var lvl) ? lvl : Models.LogLevel.Info,
                    Timestamp = dto.Timestamp,
                    CorrelationId = dto.CorrelationId,
                    TraceId = dto.TraceId,
                    SpanId = dto.SpanId,
                    UserId = dto.UserId,
                    Environment = dto.Environment,
                    Version = dto.Version,
                    Metadata = dto.Metadata,
                    Exception = !string.IsNullOrWhiteSpace(dto.Exception) ? new Exception(dto.Exception) : null,
                    Source = dto.Source,
                    Category = dto.Category
                };

                context.LogEntries.Add(log);
            }

            await context.SaveChangesAsync();
        }
    }
}

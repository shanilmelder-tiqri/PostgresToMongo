using PostgresToMongo.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Bogus;
using LogLevel = PostgresToMongo.Models.LogLevel;

namespace PostgresToMongo.Data
{
    public static class SeedData
    {
        public static List<LogEntry> GetSeedDataAsync(PostgresDbContext context, ILogger logger, int count)
        {
            var logEntryFaker = new Faker<LogEntry>()
                .RuleFor(x => x.ServiceName, f => f.PickRandom("UserService", "AuthService", "OrderService"))
                .RuleFor(x => x.LogLevel, f => f.PickRandom<LogLevel>())
                .RuleFor(x => x.Timestamp, f => f.Date.RecentOffset(3).UtcDateTime)
                .RuleFor(x => x.CorrelationId, f => $"corr-{f.Random.Guid()}")
                .RuleFor(x => x.TraceId, f => $"trace-{f.Random.Guid()}")
                .RuleFor(x => x.SpanId, f => $"span-{f.Random.Guid()}")
                .RuleFor(x => x.UserId, f => $"user{f.Random.Number(1, 1000)}")
                .RuleFor(x => x.Environment, f => f.PickRandom("Development", "Staging", "Production"))
                .RuleFor(x => x.Version, f => f.System.Semver())
                .RuleFor(x => x.Source, f => "API")
                .RuleFor(x => x.Category, f => f.PickRandom("Authentication", "Authorization", "Payment"))
                .RuleFor(x => x.Metadata, f => new Dictionary<string, object>
                {
                    ["ip"] = f.Internet.Ip(),
                    ["session"] = $"sess{f.Random.Number(1, 999)}",
                    ["device"] = f.Commerce.ProductName()
                })
                .RuleFor(x => x.Exception, f => f.Random.Bool(0.3f) ? new InvalidOperationException(f.Lorem.Sentence()) : null)
                .RuleFor(x => x.Message, (f, x) =>
                {
                    if (x.Exception != null)
                        return $"Exception occurred: {x.Exception.Message}";
                    return $"Successful {x.Category?.ToLower()} operation for user {x.UserId}.";
                });

            var logs = logEntryFaker.Generate(count);
            return logs;
        }
    }
}

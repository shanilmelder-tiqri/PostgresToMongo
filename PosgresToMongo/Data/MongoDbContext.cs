using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MongoDB.EntityFrameworkCore.Extensions;
using PostgresToMongo.Models;
using System.Text.Json;

namespace PostgresToMongo.Data
{
    public class MongoDbContext(DbContextOptions<MongoDbContext> options) : DbContext(options)
    {
        public DbSet<LogEntry> LogEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var metadataConverter = new ValueConverter<Dictionary<string, object>?, string?>(
                v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                v => v == null ? null : JsonSerializer.Deserialize<Dictionary<string, object>>(v, (JsonSerializerOptions?)null)
            );

            modelBuilder.Entity<LogEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Metadata)
                    .HasConversion(metadataConverter)
                    .HasColumnType("jsonb");
                entity.Property(e => e.Exception)
                    .HasConversion(
                        v => v == null ? null : JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                        v => v == null ? null : JsonSerializer.Deserialize<Exception>(v, (JsonSerializerOptions?)null)
                    );
            });

            modelBuilder.Entity<LogEntry>().ToCollection("log_entries");
        }
    }
}

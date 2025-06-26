using Microsoft.EntityFrameworkCore;
using PostgresToMongo.Data;
using PostgresToMongo.Models;

namespace PostgresToMongo.Repositories;

public class LogEntryRepository(PostgresDbContext postgresDbContext, MongoDbContext mongoDbContext) : ILogEntryRepository
{
    public async Task<List<LogEntry>> GetLogEntriesAsync()
    {
        return await postgresDbContext.LogEntries.ToListAsync();
    }

    public async Task SaveLogEntriesAsync(LogEntry logEntry)
    {
        await mongoDbContext.AddAsync(logEntry);
        await mongoDbContext.SaveChangesAsync();
    }
}

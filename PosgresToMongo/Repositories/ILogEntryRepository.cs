using PostgresToMongo.Models;

namespace PostgresToMongo.Repositories;

public interface ILogEntryRepository
{
    Task<List<LogEntry>> GetLogEntriesAsync();

    Task SaveLogEntriesAsync(LogEntry logEntry);
}

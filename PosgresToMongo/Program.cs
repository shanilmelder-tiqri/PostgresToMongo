using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PostgresToMongo.Data;
using PostgresToMongo.Repositories;

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production";

var configuration = new ConfigurationBuilder()
    .AddJsonFile($"appsettings.{environment}.json", optional: false, reloadOnChange: true)
    .Build();

var services = new ServiceCollection();
ConfigureServices(services, configuration);
await RunMigration(services, configuration);

Console.WriteLine(configuration.GetConnectionString("MongoDBConnection"));

Console.WriteLine("Started reading Postgres data.......");

var logEntryRepository = services.BuildServiceProvider().GetRequiredService<ILogEntryRepository>();

var logEntries = await logEntryRepository.GetLogEntriesAsync();

Console.WriteLine("Completed reading Postgres data");

if (logEntries.Count == 0)
{
    Console.WriteLine("No records found");
    return;
}

Console.WriteLine($"{logEntries.Count} records found");

Console.WriteLine("Started saving data to MongoDB.......");

foreach (var logEntry in logEntries)
{
    await logEntryRepository.SaveLogEntriesAsync(logEntry);
}

Console.WriteLine("Completed saving data to MongoDB");
return;


static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
{
    services.AddTransient<ILogEntryRepository, LogEntryRepository>();
    services.AddDbContext<PostgresDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("PostgresConnection")));
    services.AddDbContext<MongoDbContext>(options =>
        options.UseMongoDB(configuration.GetConnectionString("MongoDBConnection") ?? "", configuration["MongoDB"] ?? ""));
}

static async Task RunMigration(IServiceCollection services, IConfiguration configuration)
{
    var context = services.BuildServiceProvider().GetRequiredService<PostgresDbContext>();
    context.Database.Migrate();

    if (Convert.ToBoolean(configuration["SeedData"]))
    {
        await SeedData.SeedDataAsync(context);
    }
}
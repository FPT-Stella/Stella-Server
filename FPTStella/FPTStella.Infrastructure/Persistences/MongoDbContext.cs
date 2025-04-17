using FPTStella.Application.Common.Interfaces.Persistences;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace FPTStella.Infrastructure.Persistences
{
    public class MongoDbContext : IMongoDbContext
    {
        private readonly MongoClient _client;
        private bool _disposed;
        public IMongoDatabase Database { get; }

        public MongoDbContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("StellaConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(
                    nameof(connectionString),
                    "Connection string is missing. Please set 'ConnectionStrings:StellaConnection' in User Secrets (Development) or environment variables (Production)."
                );
            }

            var mongoUrl = new MongoUrl(connectionString);
            var settings = MongoClientSettings.FromUrl(mongoUrl);
            settings.RetryWrites = true; // Retry writes on failure
            settings.ConnectTimeout = TimeSpan.FromSeconds(30); // Timeout for connection
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(30); // Timeout for server selection

            _client = new MongoClient(settings);
            Database = _client.GetDatabase(mongoUrl.DatabaseName);
        }
        public void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
            }
        }
    }
}

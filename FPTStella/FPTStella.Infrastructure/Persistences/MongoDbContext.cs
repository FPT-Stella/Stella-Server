using FPTStella.Application.Common.Interfaces.Persistences;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;

namespace FPTStella.Infrastructure.Persistences
{
    public class MongoDbContext : IMongoDbContext
    {
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
            var client = new MongoClient(mongoUrl);
            Database = client.GetDatabase(mongoUrl.DatabaseName);
        }
    }
}

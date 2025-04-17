using FPTStella.Application.Common.Interfaces.Persistences;
using MongoDB.Driver;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;


namespace FPTStella.Infrastructure.Persistences
{
    public class MongoDbContext : IMongoDbContext, IDisposable
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
            settings.RetryWrites = true;
            settings.ConnectTimeout = TimeSpan.FromSeconds(30);
            settings.ServerSelectionTimeout = TimeSpan.FromSeconds(30);

            // Cấu hình GuidRepresentation trong MongoClientSettings (phiên bản 2.3.0)
            settings.GuidRepresentation = GuidRepresentation.Standard;

            _client = new MongoClient(settings);

            string databaseName = "StellaDb";
            if (string.IsNullOrEmpty(mongoUrl.DatabaseName))
            {
                Database = _client.GetDatabase(databaseName);
            }
            else
            {
                Database = _client.GetDatabase(mongoUrl.DatabaseName);
            }

            // Đăng ký serializer cho Guid để serialize/deserialize dưới dạng string
            if (!BsonSerializer.LookupSerializer<Guid>().GetType().Equals(typeof(GuidSerializer)))
            {
                BsonSerializer.RegisterSerializer(new GuidSerializer(BsonType.String));
            }
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

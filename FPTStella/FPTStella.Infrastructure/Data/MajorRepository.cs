using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Domain.Entities;
using FPTStella.Infrastructure.UnitOfWorks.Repositories;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.Data
{
    public class MajorRepository : Repository<Majors>, IMajorRepository
    {
        public MajorRepository(IMongoDatabase database) : base(database, "Majors")
        {
            try
            {
                // 1. First, create a partial index for non-empty MajorName values
                var indexOptions = new CreateIndexOptions<Majors>
                {
                    Unique = true,
                    Background = true,
                    PartialFilterExpression = Builders<Majors>.Filter.And(
                        Builders<Majors>.Filter.Exists(m => m.MajorName),
                        Builders<Majors>.Filter.Gt(m => m.MajorName, string.Empty),
                        Builders<Majors>.Filter.Eq(m => m.DelFlg, false)  // Only consider non-deleted records
                    )
                };

                _collection.Indexes.CreateOne(new CreateIndexModel<Majors>(
                    Builders<Majors>.IndexKeys.Ascending(m => m.MajorName),
                    indexOptions));

                // 2. Create text index for search
                _collection.Indexes.CreateOne(new CreateIndexModel<Majors>(
                    Builders<Majors>.IndexKeys
                        .Text(m => m.MajorName)
                        .Text(m => m.Description),
                    new CreateIndexOptions { Background = true }));
            }
            catch (MongoDB.Driver.MongoCommandException ex)
            {
                // Log the error
                Console.WriteLine("Error creating indexes: " + ex.Message);

                // Attempt to drop the existing index if it has a conflict
                try
                {
                    if (ex.Message.Contains("already exists"))
                    {
                        var indexCursor = _collection.Indexes.List();
                        var indexes = indexCursor.ToList();

                        foreach (var index in indexes)
                        {
                            if (index.Contains("MajorName") && index.Contains("unique"))
                            {
                                string indexName = index["name"].AsString;
                                _collection.Indexes.DropOne(indexName);
                                Console.WriteLine($"Dropped index: {indexName}");

                                // Try to create the index again, but this time without uniqueness
                                _collection.Indexes.CreateOne(new CreateIndexModel<Majors>(
                                    Builders<Majors>.IndexKeys.Ascending(m => m.MajorName),
                                    new CreateIndexOptions { Background = true }));

                                // Also create a text index for search
                                _collection.Indexes.CreateOne(new CreateIndexModel<Majors>(
                                    Builders<Majors>.IndexKeys
                                        .Text(m => m.MajorName)
                                        .Text(m => m.Description),
                                    new CreateIndexOptions { Background = true }));

                                break;
                            }
                        }
                    }
                }
                catch (Exception dropEx)
                {
                    Console.WriteLine("Error dropping/recreating indexes: " + dropEx.Message);

                    // Last resort: create non-unique indexes
                    try
                    {
                        _collection.Indexes.CreateOne(new CreateIndexModel<Majors>(
                            Builders<Majors>.IndexKeys.Ascending(m => m.MajorName),
                            new CreateIndexOptions { Background = true }));

                        _collection.Indexes.CreateOne(new CreateIndexModel<Majors>(
                            Builders<Majors>.IndexKeys
                                .Text(m => m.MajorName)
                                .Text(m => m.Description),
                            new CreateIndexOptions { Background = true }));
                    }
                    catch
                    {
                        // Ignore any further errors with index creation
                    }
                }
            }
        }
        public async Task<Majors?> GetByMajorNameAsync(string majorName)
        {
            var filter = Builders<Majors>.Filter.Eq(m => m.MajorName, majorName) &
                         Builders<Majors>.Filter.Eq(m => m.DelFlg, false);

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}

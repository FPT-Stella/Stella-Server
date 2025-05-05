using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using FPTStella.Infrastructure.UnitOfWorks.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.Data
{
    public class ProgramRepository : Repository<Programs>, IProgramRepository
    {
        private static readonly FilterDefinition<Programs> NotDeletedFilter =
            Builders<Programs>.Filter.Eq(p => p.DelFlg, false);
        public ProgramRepository(IMongoDatabase database) : base(database, "Programs")
        {
            try
            {
                // 1. First, create a partial index for non-empty ProgramCode values
                var indexOptions = new CreateIndexOptions<Programs>
                {
                    Unique = true,
                    Background = true,
                    PartialFilterExpression = Builders<Programs>.Filter.And(
                        Builders<Programs>.Filter.Exists(p => p.ProgramCode),
                        Builders<Programs>.Filter.Gt(p => p.ProgramCode, string.Empty),
                        Builders<Programs>.Filter.Eq(p => p.DelFlg, false)  // Only consider non-deleted records
                    )
                };

                _collection.Indexes.CreateOne(new CreateIndexModel<Programs>(
                    Builders<Programs>.IndexKeys.Ascending(p => p.ProgramCode),
                    indexOptions));

                // 2. Index for MajorId
                _collection.Indexes.CreateOne(new CreateIndexModel<Programs>(
                    Builders<Programs>.IndexKeys.Ascending(p => p.MajorId),
                    new CreateIndexOptions { Background = true }));

                // 3. Create text index for search
                _collection.Indexes.CreateOne(new CreateIndexModel<Programs>(
                    Builders<Programs>.IndexKeys
                        .Text(p => p.ProgramCode)
                        .Text(p => p.ProgramName)
                        .Text(p => p.Description),
                    new CreateIndexOptions { Background = true }));
            }
            catch (MongoDB.Driver.MongoCommandException ex)
            {
                // Log the error
                Console.WriteLine("Error creating indexes: " + ex.Message);

                // Attempt to drop the existing index if it has a conflict
                try
                {
                    if (ex.Message.Contains("already exists") || ex.Message.Contains("duplicate key"))
                    {
                        var indexCursor = _collection.Indexes.List();
                        var indexes = indexCursor.ToList();

                        foreach (var index in indexes)
                        {
                            if (index.Contains("program_code") && index.Contains("unique"))
                            {
                                string indexName = index["name"].AsString;
                                _collection.Indexes.DropOne(indexName);
                                Console.WriteLine($"Dropped index: {indexName}");

                                // Try to create the partial index again
                                var retryIndexOptions = new CreateIndexOptions<Programs>
                                {
                                    Unique = true,
                                    Background = true,
                                    Name = "program_code_delflg_1",
                                    PartialFilterExpression = Builders<Programs>.Filter.And(
                                        Builders<Programs>.Filter.Exists(p => p.ProgramCode),
                                        Builders<Programs>.Filter.Gt(p => p.ProgramCode, string.Empty),
                                        Builders<Programs>.Filter.Eq(p => p.DelFlg, false)
                                    )
                                };

                                _collection.Indexes.CreateOne(new CreateIndexModel<Programs>(
                                    Builders<Programs>.IndexKeys.Ascending(p => p.ProgramCode),
                                    retryIndexOptions));

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
                        _collection.Indexes.CreateOne(new CreateIndexModel<Programs>(
                            Builders<Programs>.IndexKeys.Ascending(p => p.ProgramCode),
                            new CreateIndexOptions { Background = true }));

                        _collection.Indexes.CreateOne(new CreateIndexModel<Programs>(
                            Builders<Programs>.IndexKeys.Ascending(p => p.MajorId),
                            new CreateIndexOptions { Background = true }));

                        _collection.Indexes.CreateOne(new CreateIndexModel<Programs>(
                            Builders<Programs>.IndexKeys
                                .Text(p => p.ProgramCode)
                                .Text(p => p.ProgramName)
                                .Text(p => p.Description),
                            new CreateIndexOptions { Background = true }));
                    }
                    catch
                    {
                        // Ignore any further errors with index creation
                    }
                }
            }
        }

        public async Task<Programs?> GetByProgramCodeAsync(string programCode)
        {
            if (string.IsNullOrWhiteSpace(programCode))
                return null;

            var filter = Builders<Programs>.Filter.Eq(p => p.ProgramCode, programCode) & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Programs?> GetByMajorIdAsync(Guid majorId)
        {
            var filter = Builders<Programs>.Filter.Eq(p => p.MajorId, majorId) & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Programs?> GetByProgramNameAsync(string programName)
        {
            if (string.IsNullOrWhiteSpace(programName))
                return null;

            var filter = Builders<Programs>.Filter.Eq(p => p.ProgramName, programName) & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Programs?> GetByMajorIdAndProgramNameAsync(Guid majorId, string programName)
        {
            if (string.IsNullOrWhiteSpace(programName))
                return null;

            var filter = Builders<Programs>.Filter.Eq(p => p.MajorId, majorId) &
                         Builders<Programs>.Filter.Eq(p => p.ProgramName, programName) &
                         NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Programs?> GetByMajorIdAndProgramCodeAsync(Guid majorId, string programCode)
        {
            if (string.IsNullOrWhiteSpace(programCode))
                return null;

            var filter = Builders<Programs>.Filter.Eq(p => p.MajorId, majorId) &
                         Builders<Programs>.Filter.Eq(p => p.ProgramCode, programCode) &
                         NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<PagedResult<Programs>> SearchProgramsAsync(
            string? searchTerm = null,
            Guid? majorId = null,
            PaginationParams? paginationParams = null)
        {
            var pagingParams = paginationParams ?? new PaginationParams();

            var filter = NotDeletedFilter;

            if (majorId.HasValue)
            {
                filter = filter & Builders<Programs>.Filter.Eq(p => p.MajorId, majorId.Value);
            }

            // Add search term filter if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchFilter = Builders<Programs>.Filter.Or(
                    Builders<Programs>.Filter.Regex(p => p.ProgramCode,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Programs>.Filter.Regex(p => p.ProgramName,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Programs>.Filter.Regex(p => p.Description,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")));

                filter = filter & searchFilter;
            }

            // Get total count for pagination
            var totalCount = await _collection.CountDocumentsAsync(filter);

            // Get paginated results
            var items = await _collection.Find(filter)
                .Sort(Builders<Programs>.Sort.Ascending(p => p.ProgramCode))
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Limit(pagingParams.PageSize)
                .ToListAsync();

            // Create and return paged result
            return new PagedResult<Programs>
            {
                CurrentPage = pagingParams.PageNumber,
                PageSize = pagingParams.PageSize,
                TotalCount = (int)totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pagingParams.PageSize),
                Items = items
            };
        }
    }
}

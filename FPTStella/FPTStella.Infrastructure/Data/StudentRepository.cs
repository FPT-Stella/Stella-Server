using FPTStella.Application.Common.Interfaces.Persistences;
using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
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
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        private static readonly FilterDefinition<Student> NotDeletedFilter =
            Builders<Student>.Filter.Eq(s => s.DelFlg, false);
        public StudentRepository(IMongoDatabase database) : base(database, "Student")
        {
            try
            {
                // 1. First, create a partial index for non-empty StudentCode values
                var indexOptions = new CreateIndexOptions<Student>
                {
                    Unique = true,
                    Background = true,
                    PartialFilterExpression = Builders<Student>.Filter.And(
                        Builders<Student>.Filter.Exists(s => s.StudentCode),
                        Builders<Student>.Filter.Gt(s => s.StudentCode, string.Empty)
                    )
                };

                _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                    Builders<Student>.IndexKeys.Ascending(s => s.StudentCode),
                    indexOptions));

                // 2. Proceed with other indexes
                _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                    Builders<Student>.IndexKeys.Ascending(s => s.UserId),
                    new CreateIndexOptions { Background = true }));

                _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                    Builders<Student>.IndexKeys.Ascending(s => s.MajorId),
                    new CreateIndexOptions { Background = true }));

                // Index for text search
                _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                    Builders<Student>.IndexKeys
                        .Text(s => s.StudentCode)
                        .Text(s => s.Phone)
                        .Text(s => s.Address),
                    new CreateIndexOptions { Background = true }));
            }
            catch (MongoDB.Driver.MongoCommandException ex)
            {
                // Log the error
                Console.WriteLine("Error creating indexes: " + ex.Message);

                // Create non-unique index as fallback if unique index fails
                _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                    Builders<Student>.IndexKeys.Ascending(s => s.StudentCode),
                    new CreateIndexOptions { Background = true }));
                _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                    Builders<Student>.IndexKeys.Ascending(s => s.UserId),
                    new CreateIndexOptions { Background = true }));
                _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                    Builders<Student>.IndexKeys.Ascending(s => s.MajorId),
                    new CreateIndexOptions { Background = true }));
                _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                    Builders<Student>.IndexKeys
                        .Text(s => s.StudentCode)
                        .Text(s => s.Phone)
                        .Text(s => s.Address),
                    new CreateIndexOptions { Background = true }));
            }
        }
        public async Task<Student?> GetByStudentCodeAsync(string studentCode)
        {
            return await FindOneAsync(s => s.StudentCode == studentCode && s.DelFlg == false);
        }

        public async Task<Student?> GetByUserIdAsync(Guid userId)
        {
            return await FindOneAsync(s => s.UserId == userId && s.DelFlg == false);
        }
        public async Task<PagedResult<Student>> SearchStudentsAsync(
    string? searchTerm = null,
    Guid? majorId = null,
    PaginationParams? paginationParams = null)
        {
            // Use provided pagination params or create default
            var pagingParams = paginationParams ?? new PaginationParams();

            // Start with not deleted filter
            var filter = Builders<Student>.Filter.Eq(s => s.DelFlg, false);

            // Add majorId filter if provided
            if (majorId.HasValue)
            {
                filter = filter & Builders<Student>.Filter.Eq(s => s.MajorId, majorId.Value);
            }

            // Add search term filter if provided
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchFilter = Builders<Student>.Filter.Or(
                    Builders<Student>.Filter.Regex(s => s.StudentCode,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Student>.Filter.Regex(s => s.Phone,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Student>.Filter.Regex(s => s.Address,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")));

                filter = filter & searchFilter;
            }

            // Get total count for pagination
            var totalCount = await _collection.CountDocumentsAsync(filter);

            // Get paginated results
            var items = await _collection.Find(filter)
                .Sort(Builders<Student>.Sort.Ascending(s => s.StudentCode))
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Limit(pagingParams.PageSize)
                .ToListAsync();

            // Create and return paged result
            return new PagedResult<Student>
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

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
    public class MaterialRepository : Repository<Materials>, IMaterialRepository
    {
        private static readonly FilterDefinition<Materials> NotDeletedFilter =
            Builders<Materials>.Filter.Eq(m => m.DelFlg, false);
        public MaterialRepository(IMongoDatabase database) : base(database, "Materials")
        {
            try
            {
                var nameIndexOptions = new CreateIndexOptions<Materials>
                {
                    Unique = true,
                    Background = true,
                    PartialFilterExpression = Builders<Materials>.Filter.And(
                        Builders<Materials>.Filter.Exists(m => m.MaterialName),
                        Builders<Materials>.Filter.Gt(m => m.MaterialName, string.Empty)
                    )
                };

                _collection.Indexes.CreateOne(new CreateIndexModel<Materials>(
                    Builders<Materials>.IndexKeys.Ascending(m => m.MaterialName),
                    nameIndexOptions));

                // Create index for subject_id for faster lookup
                _collection.Indexes.CreateOne(new CreateIndexModel<Materials>(
                    Builders<Materials>.IndexKeys.Ascending(m => m.SubjectId),
                    new CreateIndexOptions { Background = true }));

                // Create index for material_type for filtering
                _collection.Indexes.CreateOne(new CreateIndexModel<Materials>(
                    Builders<Materials>.IndexKeys.Ascending(m => m.MaterialType),
                    new CreateIndexOptions { Background = true }));

                // Create text index for search
                _collection.Indexes.CreateOne(new CreateIndexModel<Materials>(
                    Builders<Materials>.IndexKeys
                        .Text(m => m.MaterialName)
                        .Text(m => m.Description)
                        .Text(m => m.MaterialType),
                    new CreateIndexOptions { Background = true }));
            }
            catch (MongoDB.Driver.MongoCommandException ex)
            {
                Console.WriteLine("Error creating indexes: " + ex.Message);
                // Create non-unique index as fallback if unique index fails
                _collection.Indexes.CreateOne(new CreateIndexModel<Materials>(
                    Builders<Materials>.IndexKeys.Ascending(m => m.MaterialName),
                    new CreateIndexOptions { Background = true }));
                //Các index khác
                _collection.Indexes.CreateOne(new CreateIndexModel<Materials>(
                    Builders<Materials>.IndexKeys.Ascending(m => m.SubjectId),
                    new CreateIndexOptions { Background = true }));
                _collection.Indexes.CreateOne(new CreateIndexModel<Materials>(
                    Builders<Materials>.IndexKeys.Ascending(m => m.MaterialType),
                    new CreateIndexOptions { Background = true }));
                _collection.Indexes.CreateOne(new CreateIndexModel<Materials>(
                    Builders<Materials>.IndexKeys
                        .Text(m => m.MaterialName)
                        .Text(m => m.Description)
                        .Text(m => m.MaterialType),
                    new CreateIndexOptions { Background = true }));
            }
        }
        /// <summary>
        /// Gets materials by subject ID
        /// </summary>
        public async Task<List<Materials>> GetBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<Materials>.Filter.Eq(m => m.SubjectId, subjectId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }
        /// <summary>
        /// Gets a material by its name
        /// </summary>
        public async Task<Materials?> GetByMaterialNameAsync(string materialName)
        {
            return await FindOneAsync(m => m.MaterialName == materialName && m.DelFlg == false);
        }
        /// <summary>
        /// Gets materials by material type
        /// </summary>
        public async Task<List<Materials>> GetByMaterialTypeAsync(string materialType)
        {
            var filter = Builders<Materials>.Filter.Eq(m => m.MaterialType, materialType) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }
        /// <summary>
        /// Searches for materials with advanced filtering options and pagination
        /// </summary>
        public async Task<PagedResult<Materials>> SearchMaterialsAsync(
            string? searchTerm = null,
            Guid? subjectId = null,
            string? materialType = null,
            PaginationParams? paginationParams = null)
        {          
            var pagingParams = paginationParams ?? new PaginationParams();
            var filter = NotDeletedFilter;
            if (subjectId.HasValue)
            {
                filter = filter & Builders<Materials>.Filter.Eq(m => m.SubjectId, subjectId.Value);
            }
            if (!string.IsNullOrWhiteSpace(materialType))
            {
                filter = filter & Builders<Materials>.Filter.Eq(m => m.MaterialType, materialType);
            }
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var searchFilter = Builders<Materials>.Filter.Or(
                    Builders<Materials>.Filter.Regex(m => m.MaterialName,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Materials>.Filter.Regex(m => m.Description,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")),
                    Builders<Materials>.Filter.Regex(m => m.MaterialType,
                        new MongoDB.Bson.BsonRegularExpression(searchTerm, "i")));

                filter = filter & searchFilter;
            }
            var totalCount = await _collection.CountDocumentsAsync(filter);
            var items = await _collection.Find(filter)
                .Sort(Builders<Materials>.Sort.Ascending(m => m.MaterialName))
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Limit(pagingParams.PageSize)
                .ToListAsync();

            return new PagedResult<Materials>
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

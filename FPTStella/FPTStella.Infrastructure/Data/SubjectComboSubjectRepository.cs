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
    public class SubjectComboSubjectRepository : Repository<SubjectComboSubjects>, ISubjectComboSubjectRepository
    {
        private static readonly FilterDefinition<SubjectComboSubjects> NotDeletedFilter =
            Builders<SubjectComboSubjects>.Filter.Eq(s => s.DelFlg, false);
        public SubjectComboSubjectRepository(IMongoDatabase database) : base(database, "SubjectComboSubjects")
        {
            CreateIndexes();
        }
        private void CreateIndexes()
        {
            try
            {
                // First single-field index
                _collection.Indexes.CreateOne(new CreateIndexModel<SubjectComboSubjects>(
                    Builders<SubjectComboSubjects>.IndexKeys.Ascending(s => s.SubjectComboId),
                    new CreateIndexOptions { Name = "idx_subjectComboId", Background = true }));

                // Second single-field index
                _collection.Indexes.CreateOne(new CreateIndexModel<SubjectComboSubjects>(
                    Builders<SubjectComboSubjects>.IndexKeys.Ascending(s => s.SubjectId),
                    new CreateIndexOptions { Name = "idx_subjectId", Background = true }));

                // Unique composite index
                var uniqueIndexOptions = new CreateIndexOptions<SubjectComboSubjects>
                {
                    Name = "idx_unique_combo_subject_active",
                    Unique = true,
                    Background = true,
                    PartialFilterExpression = Builders<SubjectComboSubjects>.Filter.Eq(s => s.DelFlg, false)
                };

                _collection.Indexes.CreateOne(new CreateIndexModel<SubjectComboSubjects>(
                    Builders<SubjectComboSubjects>.IndexKeys
                        .Ascending(s => s.SubjectComboId)
                        .Ascending(s => s.SubjectId),
                    uniqueIndexOptions));
            }
            catch (MongoDB.Driver.MongoCommandException ex)
            {
                Console.WriteLine($"Error creating indexes: {ex.Message}");

                // Use a different name for the fallback index
                _collection.Indexes.CreateOne(new CreateIndexModel<SubjectComboSubjects>(
                    Builders<SubjectComboSubjects>.IndexKeys
                        .Ascending(s => s.SubjectComboId)
                        .Ascending(s => s.SubjectId),
                    new CreateIndexOptions { Name = "idx_non_unique_combo_subject", Background = true }));
            }
        }
        /// <summary>
        /// Gets all subject mappings for a specific subject combo
        /// </summary>
        public async Task<List<SubjectComboSubjects>> GetBySubjectComboIdAsync(Guid subjectComboId)
        {
            var filter = Builders<SubjectComboSubjects>.Filter.Eq(s => s.SubjectComboId, subjectComboId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Gets all combo mappings for a specific subject
        /// </summary>
        public async Task<List<SubjectComboSubjects>> GetBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<SubjectComboSubjects>.Filter.Eq(s => s.SubjectId, subjectId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Checks if a mapping between a subject combo and a subject already exists
        /// </summary>
        public async Task<bool> IsMappingExistedAsync(Guid subjectComboId, Guid subjectId)
        {
            var filter = Builders<SubjectComboSubjects>.Filter.Eq(s => s.SubjectComboId, subjectComboId) &
                         Builders<SubjectComboSubjects>.Filter.Eq(s => s.SubjectId, subjectId) &
                         NotDeletedFilter;

            return await _collection.CountDocumentsAsync(filter) > 0;
        }

        /// <summary>
        /// Removes all subject mappings for a specific subject combo
        /// </summary>
        public async Task DeleteMappingsBySubjectComboIdAsync(Guid subjectComboId)
        {
            var filter = Builders<SubjectComboSubjects>.Filter.Eq(s => s.SubjectComboId, subjectComboId) & NotDeletedFilter;
            var update = Builders<SubjectComboSubjects>.Update
                .Set(s => s.DelFlg, true)
                .Set(s => s.UpdDate, DateTime.UtcNow);

            await _collection.UpdateManyAsync(filter, update);
        }

        /// <summary>
        /// Removes all combo mappings for a specific subject
        /// </summary>
        public async Task DeleteMappingsBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<SubjectComboSubjects>.Filter.Eq(s => s.SubjectId, subjectId) & NotDeletedFilter;
            var update = Builders<SubjectComboSubjects>.Update
                .Set(s => s.DelFlg, true)
                .Set(s => s.UpdDate, DateTime.UtcNow);

            await _collection.UpdateManyAsync(filter, update);
        }

        /// <summary>
        /// Gets all subject IDs associated with a specific subject combo
        /// </summary>
        public async Task<List<Guid>> GetSubjectIdsBySubjectComboIdAsync(Guid subjectComboId)
        {
            var filter = Builders<SubjectComboSubjects>.Filter.Eq(s => s.SubjectComboId, subjectComboId) & NotDeletedFilter;
            var projection = Builders<SubjectComboSubjects>.Projection.Include(s => s.SubjectId).Exclude(s => s.Id);

            var results = await _collection.Find(filter)
                                       .Project<SubjectComboSubjects>(projection)
                                       .ToListAsync();

            return results.Select(s => s.SubjectId).ToList();
        }

        /// <summary>
        /// Gets all subject combo IDs that contain a specific subject
        /// </summary>
        public async Task<List<Guid>> GetSubjectComboIdsBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<SubjectComboSubjects>.Filter.Eq(s => s.SubjectId, subjectId) & NotDeletedFilter;
            var projection = Builders<SubjectComboSubjects>.Projection.Include(s => s.SubjectComboId).Exclude(s => s.Id);

            var results = await _collection.Find(filter)
                                       .Project<SubjectComboSubjects>(projection)
                                       .ToListAsync();

            return results.Select(s => s.SubjectComboId).ToList();
        }

        /// <summary>
        /// Searches for mappings with pagination and filtering.
        /// If no filters are provided, returns all active records (similar to GetAllAsync).
        /// </summary>
        public async Task<PagedResult<SubjectComboSubjects>> SearchMappingsAsync(
            Guid? subjectComboId = null,
            Guid? subjectId = null,
            PaginationParams? paginationParams = null)
        {
            // Use provided pagination params or create default
            var pagingParams = paginationParams ?? new PaginationParams();

            // Start with just the not deleted filter
            var filter = NotDeletedFilter;

            // Only add specific filters if they have values
            if (subjectComboId.HasValue)
            {
                filter = filter & Builders<SubjectComboSubjects>.Filter.Eq(s => s.SubjectComboId, subjectComboId.Value);
            }

            if (subjectId.HasValue)
            {
                filter = filter & Builders<SubjectComboSubjects>.Filter.Eq(s => s.SubjectId, subjectId.Value);
            }

            // Get total count for pagination
            var totalCount = await _collection.CountDocumentsAsync(filter);

            // Get paginated results
            var items = await _collection.Find(filter)
                .Skip((pagingParams.PageNumber - 1) * pagingParams.PageSize)
                .Limit(pagingParams.PageSize)
                .ToListAsync();

            // Create and return paged result
            return new PagedResult<SubjectComboSubjects>
            {
                CurrentPage = pagingParams.PageNumber,
                PageSize = pagingParams.PageSize,
                TotalCount = (int)totalCount,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pagingParams.PageSize),
                Items = items
            };
        }
        // Add to SubjectComboSubjectRepository.cs
        public async Task<SubjectComboSubjects?> GetMappingAsync(Guid subjectComboId, Guid subjectId)
        {
            var filter = Builders<SubjectComboSubjects>.Filter.Eq(m => m.SubjectComboId, subjectComboId) &
                         Builders<SubjectComboSubjects>.Filter.Eq(m => m.SubjectId, subjectId) &
                         NotDeletedFilter;

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(SubjectComboSubjects mapping)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(mapping));
            }

            mapping.UpdDate = DateTime.UtcNow;

            // Create a filter to find the document by ID
            var filter = Builders<SubjectComboSubjects>.Filter.Eq(m => m.Id, mapping.Id);

            // Replace the existing document with the updated one
            await _collection.ReplaceOneAsync(filter, mapping);
        }

        public async Task UpdateManyAsync(IEnumerable<SubjectComboSubjects> mappings)
        {
            if (mappings == null)
            {
                throw new ArgumentNullException(nameof(mappings));
            }

            foreach (var mapping in mappings)
            {
                await UpdateAsync(mapping);
            }
        }

        public async Task AddManyAsync(IEnumerable<SubjectComboSubjects> mappings)
        {
            if (mappings == null)
            {
                throw new ArgumentNullException(nameof(mappings));
            }

            // Filter out duplicates by checking if the mapping already exists
            var uniqueMappings = new List<SubjectComboSubjects>();
            foreach (var mapping in mappings)
            {
                var exists = await IsMappingExistedAsync(mapping.SubjectComboId, mapping.SubjectId);
                if (!exists)
                {
                    uniqueMappings.Add(mapping);
                }
            }

            if (uniqueMappings.Any())
            {
                try
                {
                    await _collection.InsertManyAsync(uniqueMappings);
                }
                catch (MongoWriteException ex) when (ex.WriteError?.Category == ServerErrorCategory.DuplicateKey)
                {
                    // Log and ignore duplicate key errors
                    Console.WriteLine($"Duplicate key error: {ex.Message}");
                }
            }
        }
    }
}

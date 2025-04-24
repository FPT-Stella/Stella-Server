using FPTStella.Application.Common.Interfaces.Repositories;
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
    public class SubjectInCurriculumRepository : Repository<SubjectInCurriculum>, ISubjectInCurriculumRepository
    {
        public static readonly FilterDefinition<SubjectInCurriculum> NotDeletedFilter =
            Builders<SubjectInCurriculum>.Filter.Eq(m => m.DelFlg, false);
        public SubjectInCurriculumRepository(IMongoDatabase database) : base(database, "SubjectInCurriculum")
        {
            CreateIndexes();
        }
        /// <summary>
        /// Creates indexes for the SubjectInCurriculum collection.
        /// </summary>
        private void CreateIndexes()
        {
            // Create a unique compound index for SubjectId and CurriculumId
            _collection.Indexes.CreateOne(new CreateIndexModel<SubjectInCurriculum>(
                Builders<SubjectInCurriculum>.IndexKeys
                    .Ascending(m => m.SubjectId)
                    .Ascending(m => m.CurriculumId),
                new CreateIndexOptions { Unique = true }));

            // Create an index for searching by CurriculumId
            _collection.Indexes.CreateOne(new CreateIndexModel<SubjectInCurriculum>(
                Builders<SubjectInCurriculum>.IndexKeys.Ascending(m => m.CurriculumId)));

            // Create an index for searching by SubjectId
            _collection.Indexes.CreateOne(new CreateIndexModel<SubjectInCurriculum>(
                Builders<SubjectInCurriculum>.IndexKeys.Ascending(m => m.SubjectId)));
        }
        /// <summary>
        /// Gets subjects in a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>A list of SubjectInCurriculum entities</returns>
        public async Task<List<SubjectInCurriculum>> GetByCurriculumIdAsync(Guid curriculumId)
        {
            var filter = Builders<SubjectInCurriculum>.Filter.Eq(m => m.CurriculumId, curriculumId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Gets curricula that contain a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>A list of SubjectInCurriculum entities</returns>
        public async Task<List<SubjectInCurriculum>> GetBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<SubjectInCurriculum>.Filter.Eq(m => m.SubjectId, subjectId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Checks if a subject is already mapped to a curriculum.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>True if mapping exists, otherwise false</returns>
        public async Task<bool> IsMappingExistedAsync(Guid subjectId, Guid curriculumId)
        {
            var filter = Builders<SubjectInCurriculum>.Filter.Eq(m => m.SubjectId, subjectId) &
                        Builders<SubjectInCurriculum>.Filter.Eq(m => m.CurriculumId, curriculumId) &
                        NotDeletedFilter;

            return await _collection.CountDocumentsAsync(filter) > 0;
        }

        /// <summary>
        /// Soft deletes all mappings for a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        public async Task DeleteMappingsBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<SubjectInCurriculum>.Filter.Eq(m => m.SubjectId, subjectId) & NotDeletedFilter;
            var update = Builders<SubjectInCurriculum>.Update
                .Set(m => m.DelFlg, true)
                .Set(m => m.UpdDate, DateTime.UtcNow);

            await _collection.UpdateManyAsync(filter, update);
        }

        /// <summary>
        /// Soft deletes all mappings for a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        public async Task DeleteMappingsByCurriculumIdAsync(Guid curriculumId)
        {
            var filter = Builders<SubjectInCurriculum>.Filter.Eq(m => m.CurriculumId, curriculumId) & NotDeletedFilter;
            var update = Builders<SubjectInCurriculum>.Update
                .Set(m => m.DelFlg, true)
                .Set(m => m.UpdDate, DateTime.UtcNow);

            await _collection.UpdateManyAsync(filter, update);
        }

        /// <summary>
        /// Gets all SubjectInCurriculum mappings that match both subject IDs and curriculum IDs.
        /// </summary>
        /// <param name="subjectIds">List of subject IDs</param>
        /// <param name="curriculumIds">List of curriculum IDs</param>
        /// <returns>A list of SubjectInCurriculum entities</returns>
        public async Task<List<SubjectInCurriculum>> GetBySubjectIdsAndCurriculumIdsAsync(List<Guid> subjectIds, List<Guid> curriculumIds)
        {
            var filter = Builders<SubjectInCurriculum>.Filter.In(m => m.SubjectId, subjectIds) &
                         Builders<SubjectInCurriculum>.Filter.In(m => m.CurriculumId, curriculumIds) &
                         NotDeletedFilter;

            return await _collection.Find(filter).ToListAsync();
        }

        /// <summary>
        /// Gets the count of subjects in a curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>The number of subjects in the curriculum</returns>
        public async Task<long> GetSubjectCountByCurriculumIdAsync(Guid curriculumId)
        {
            var filter = Builders<SubjectInCurriculum>.Filter.Eq(m => m.CurriculumId, curriculumId) & NotDeletedFilter;
            return await _collection.CountDocumentsAsync(filter);
        }

        /// <summary>
        /// Gets the count of curricula that contain a subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>The number of curricula containing the subject</returns>
        public async Task<long> GetCurriculumCountBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<SubjectInCurriculum>.Filter.Eq(m => m.SubjectId, subjectId) & NotDeletedFilter;
            return await _collection.CountDocumentsAsync(filter);
        }

        /// <summary>
        /// Gets a list of curriculum IDs associated with a specific subject ID.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of curriculum IDs</returns>
        public async Task<List<Guid>> GetCurriculumIdsBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<SubjectInCurriculum>.Filter.Eq(m => m.SubjectId, subjectId) & NotDeletedFilter;
            var projection = Builders<SubjectInCurriculum>.Projection.Include(m => m.CurriculumId).Exclude(m => m.Id);

            var results = await _collection.Find(filter)
                                        .Project<SubjectInCurriculum>(projection)
                                        .ToListAsync();

            return results.Select(m => m.CurriculumId).ToList();
        }

        /// <summary>
        /// Gets a list of subject IDs associated with a specific curriculum ID.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>List of subject IDs</returns>
        public async Task<List<Guid>> GetSubjectIdsByCurriculumIdAsync(Guid curriculumId)
        {
            var filter = Builders<SubjectInCurriculum>.Filter.Eq(m => m.CurriculumId, curriculumId) & NotDeletedFilter;
            var projection = Builders<SubjectInCurriculum>.Projection.Include(m => m.SubjectId).Exclude(m => m.Id);

            var results = await _collection.Find(filter)
                                        .Project<SubjectInCurriculum>(projection)
                                        .ToListAsync();

            return results.Select(m => m.SubjectId).ToList();
        }
    }
}

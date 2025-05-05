using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Domain.Entities;
using FPTStella.Infrastructure.UnitOfWorks.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.Data
{
    public class CLORepository : Repository<CLOs>, ICLORepository
    {
        private static readonly FilterDefinition<CLOs> NotDeletedFilter =
            Builders<CLOs>.Filter.Eq(c => c.DelFlg, false);

        private void CreateIndexes()
        {
            try
            {
                // Unique index for SubjectId and CloName using partial filter
                var cloNameIndexOptions = new CreateIndexOptions<CLOs>
                {
                    Unique = true,
                    Background = true,
                    PartialFilterExpression = Builders<CLOs>.Filter.And(
                        Builders<CLOs>.Filter.Exists(c => c.CloName),
                        Builders<CLOs>.Filter.Ne(c => c.CloName, string.Empty),
                        Builders<CLOs>.Filter.Ne(c => c.CloName, null)
                    )
                };

                _collection.Indexes.CreateOne(new CreateIndexModel<CLOs>(
                    Builders<CLOs>.IndexKeys
                        .Ascending(c => c.SubjectId)
                        .Ascending(c => c.CloName),
                    cloNameIndexOptions));

                // Unique index for SubjectId and CloDetails using partial filter
                var cloDetailsIndexOptions = new CreateIndexOptions<CLOs>
                {
                    Unique = true,
                    Background = true,
                    PartialFilterExpression = Builders<CLOs>.Filter.And(
                        Builders<CLOs>.Filter.Exists(c => c.CloDetails),
                        Builders<CLOs>.Filter.Ne(c => c.CloDetails, string.Empty),
                        Builders<CLOs>.Filter.Ne(c => c.CloDetails, null)
                    )
                };

                _collection.Indexes.CreateOne(new CreateIndexModel<CLOs>(
                    Builders<CLOs>.IndexKeys
                        .Ascending(c => c.SubjectId)
                        .Ascending(c => c.CloDetails),
                    cloDetailsIndexOptions));

                // Index for searching by SubjectId
                _collection.Indexes.CreateOne(new CreateIndexModel<CLOs>(
                    Builders<CLOs>.IndexKeys.Ascending(c => c.SubjectId)));
            }
            catch (MongoCommandException ex)
            {
                Console.WriteLine($"Error creating indexes: {ex.Message}");

                _collection.Indexes.CreateOne(new CreateIndexModel<CLOs>(
                    Builders<CLOs>.IndexKeys.Ascending(c => c.SubjectId)));
            }
        }

        public CLORepository(IMongoDatabase database) : base(database, "CLOs")
        {
            CreateIndexes();
        }

        public async Task<List<CLOs>> GetBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<CLOs>.Filter.Eq(c => c.SubjectId, subjectId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> IsCloNameExistedAsync(Guid subjectId, string cloName)
        {
            var filter = Builders<CLOs>.Filter.Eq(c => c.SubjectId, subjectId) &
                         Builders<CLOs>.Filter.Eq(c => c.CloName, cloName) &
                         NotDeletedFilter;
            return await _collection.CountDocumentsAsync(filter) > 0;
        }

        public async Task<bool> IsCloDetailsExistedAsync(Guid subjectId, string cloDetails)
        {
            var filter = Builders<CLOs>.Filter.Eq(c => c.SubjectId, subjectId) &
                         Builders<CLOs>.Filter.Eq(c => c.CloDetails, cloDetails) &
                         NotDeletedFilter;
            return await _collection.CountDocumentsAsync(filter) > 0;
        }

        public async Task<List<CLOs>> GetBySubjectIdsAsync(List<Guid> subjectIds)
        {
            var filter = Builders<CLOs>.Filter.In(c => c.SubjectId, subjectIds) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task DeleteBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<CLOs>.Filter.Eq(c => c.SubjectId, subjectId) & NotDeletedFilter;
            var update = Builders<CLOs>.Update.Set(c => c.DelFlg, true)
                                              .Set(c => c.UpdDate, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update);
        }
    }
}
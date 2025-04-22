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
    public class PLORepository : Repository<PLOs>, IPLORepository
    {
        private static readonly FilterDefinition<PLOs> NotDeletedFilter =
            Builders<PLOs>.Filter.Eq(p => p.DelFlg, false);
        private void CreateIndexes()
        {
            // Index duy nhất cho CurriculumId và PloName
            _collection.Indexes.CreateOne(new CreateIndexModel<PLOs>(
                Builders<PLOs>.IndexKeys
                    .Ascending(p => p.CurriculumId)
                    .Ascending(p => p.PloName),
                new CreateIndexOptions { Unique = true }));

            // Index tìm kiếm theo CurriculumId
            _collection.Indexes.CreateOne(new CreateIndexModel<PLOs>(
                Builders<PLOs>.IndexKeys.Ascending(p => p.CurriculumId)));
        }

        public PLORepository(IMongoDatabase database) : base(database, "PLOs")
        {
            CreateIndexes();
        }

        public async Task<List<PLOs>> GetByCurriculumIdAsync(Guid curriculumId)
        {
            var filter = Builders<PLOs>.Filter.Eq(p => p.CurriculumId, curriculumId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }
        public async Task<bool> IsPloNameExistedAsync(Guid curriculumId, string ploName)
        {
            var filter = Builders<PLOs>.Filter.Eq(p => p.CurriculumId, curriculumId) &
                         Builders<PLOs>.Filter.Eq(p => p.PloName, ploName) &
                         Builders<PLOs>.Filter.Eq(p => p.DelFlg, false);
            return await _collection.CountDocumentsAsync(filter) > 0;
        }
        public async Task<List<PLOs>> GetByCurriculumIdsAsync(List<Guid> curriculumIds)
        {
            var filter = Builders<PLOs>.Filter.In(p => p.CurriculumId, curriculumIds) &
                         Builders<PLOs>.Filter.Eq(p => p.DelFlg, false);
            return await _collection.Find(filter).ToListAsync();
        }
        public async Task DeleteByCurriculumIdAsync(Guid curriculumId)
        {
            var filter = Builders<PLOs>.Filter.Eq(p => p.CurriculumId, curriculumId) &
                         Builders<PLOs>.Filter.Eq(p => p.DelFlg, false);
            var update = Builders<PLOs>.Update.Set(p => p.DelFlg, true)
                                              .Set(p => p.UpdDate, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update);
        }
    }
}

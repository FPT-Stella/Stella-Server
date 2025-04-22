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
    public class PORepository : Repository<POs>, IPORepository
    {
        private static readonly FilterDefinition<POs> NotDeletedFilter =
            Builders<POs>.Filter.Eq(p => p.DelFlg, false);

        public PORepository(IMongoDatabase database) : base(database, "POs")
        {
        }

        public async Task<List<POs>> GetByProgramIdAsync(Guid programId)
        {
            var filter = Builders<POs>.Filter.Eq(p => p.ProgramId, programId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }
        public async Task<bool> IsPoNameExistedAsync(Guid programId, string poName)
        {
            var filter = Builders<POs>.Filter.Eq(p => p.ProgramId, programId) &
                         Builders<POs>.Filter.Eq(p => p.PoName, poName) &
                         Builders<POs>.Filter.Eq(p => p.DelFlg, false);
            return await _collection.CountDocumentsAsync(filter) > 0;
        }
        public async Task<List<POs>> GetByProgramIdsAsync(List<Guid> programIds)
        {
            var filter = Builders<POs>.Filter.In(p => p.ProgramId, programIds) &
                         Builders<POs>.Filter.Eq(p => p.DelFlg, false);
            return await _collection.Find(filter).ToListAsync();
        }
        public async Task DeleteByProgramIdAsync(Guid programId)
        {
            var filter = Builders<POs>.Filter.Eq(p => p.ProgramId, programId) &
                         Builders<POs>.Filter.Eq(p => p.DelFlg, false);
            var update = Builders<POs>.Update.Set(p => p.DelFlg, true)
                                             .Set(p => p.UpdDate, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update);
        }
    }
}

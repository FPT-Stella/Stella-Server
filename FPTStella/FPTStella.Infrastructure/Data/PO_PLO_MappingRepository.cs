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
    public class PO_PLO_MappingRepository : Repository<PO_PLO_Mapping>, IPO_PLO_MappingRepository
    {
        private static readonly FilterDefinition<PO_PLO_Mapping> NotDeletedFilter =
            Builders<PO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);

        public PO_PLO_MappingRepository(IMongoDatabase database) : base(database, "PO_PLO_Mapping")
        {
        }

        public async Task<List<PO_PLO_Mapping>> GetByPoIdAsync(Guid poId)
        {
            var filter = Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PoId, poId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<PO_PLO_Mapping>> GetByPloIdAsync(Guid ploId)
        {
            var filter = Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }
        public async Task<bool> IsMappingExistedAsync(Guid poId, Guid ploId)
        {
            var filter = Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PoId, poId) &
                         Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) &
                         Builders<PO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            return await _collection.CountDocumentsAsync(filter) > 0;
        }
        public async Task DeleteMappingsByPoIdAsync(Guid poId)
        {
            var filter = Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PoId, poId) &
                         Builders<PO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var update = Builders<PO_PLO_Mapping>.Update.Set(m => m.DelFlg, true)
                                                        .Set(m => m.UpdDate, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update);
        }
        public async Task DeleteMappingsByPloIdAsync(Guid ploId)
        {
            var filter = Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) &
                         Builders<PO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var update = Builders<PO_PLO_Mapping>.Update.Set(m => m.DelFlg, true)
                                                        .Set(m => m.UpdDate, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update);
        }
        public async Task<List<Guid>> GetPloIdsByPoIdAsync(Guid poId)
        {
            var filter = Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PoId, poId) &
                         Builders<PO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var projection = Builders<PO_PLO_Mapping>.Projection.Expression(m => m.PloId);
            return await _collection.Find(filter).Project(projection).ToListAsync();
        }
        public async Task<List<Guid>> GetPoIdsByPloIdAsync(Guid ploId)
        {
            var filter = Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) &
                         Builders<PO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var projection = Builders<PO_PLO_Mapping>.Projection.Expression(m => m.PoId);
            return await _collection.Find(filter).Project(projection).ToListAsync();
        }
    }
}

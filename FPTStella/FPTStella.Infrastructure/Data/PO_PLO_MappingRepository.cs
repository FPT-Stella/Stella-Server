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
        public async Task<List<(Guid Id, string Name)>> GetPOsWithNameByPloIdAsync(Guid ploId)
        {
            // Step 1: Get PO IDs from mappings
            var filter = Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) &
                         Builders<PO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var mappings = await _collection.Find(filter).ToListAsync();

            if (!mappings.Any())
            {
                return new List<(Guid, string)>();
            }

            var poIds = mappings.Select(m => m.PoId).ToList();

            // Step 2: Get PO details from POs collection
            // Access the MongoDB database through the _collection's Database property
            var poCollection = _collection.Database.GetCollection<POs>("POs");
            var poFilter = Builders<POs>.Filter.In(p => p.Id, poIds) &
                          Builders<POs>.Filter.Eq(p => p.DelFlg, false);

            var pos = await poCollection.Find(poFilter).ToListAsync();

            // Step 3: Map to result format
            return pos.Select(p => (p.Id, p.PoName)).ToList();
        }
    }
}

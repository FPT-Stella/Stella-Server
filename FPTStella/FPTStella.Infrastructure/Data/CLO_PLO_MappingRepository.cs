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
    public class CLO_PLO_MappingRepository : Repository<CLO_PLO_Mapping>, ICLO_PLO_MappingRepository
    {
        private static readonly FilterDefinition<CLO_PLO_Mapping> NotDeletedFilter =
            Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);

        public CLO_PLO_MappingRepository(IMongoDatabase database) : base(database, "CLO_PLO_Mapping")
        {
        }

        public async Task<List<CLO_PLO_Mapping>> GetByCloIdAsync(Guid cloId)
        {
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.CloId, cloId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<CLO_PLO_Mapping>> GetByPloIdAsync(Guid ploId)
        {
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> IsMappingExistedAsync(Guid cloId, Guid ploId)
        {
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.CloId, cloId) &
                         Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) &
                         Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            return await _collection.CountDocumentsAsync(filter) > 0;
        }

        public async Task DeleteMappingsByCloIdAsync(Guid cloId)
        {
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.CloId, cloId) &
                         Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var update = Builders<CLO_PLO_Mapping>.Update.Set(m => m.DelFlg, true)
                                                        .Set(m => m.UpdDate, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update);

        }
        public async Task DeleteMappingsByPloIdAsync(Guid ploId)
        {
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) &
                         Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var update = Builders<CLO_PLO_Mapping>.Update.Set(m => m.DelFlg, true)
                                                        .Set(m => m.UpdDate, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update);
        }

        public async Task<List<Guid>> GetPloIdsByCloIdAsync(Guid cloId)
        {
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.CloId, cloId) &
                         Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var projection = Builders<CLO_PLO_Mapping>.Projection.Expression(m => m.PloId);
            return await _collection.Find(filter).Project(projection).ToListAsync();
        }

        public async Task<List<Guid>> GetCloIdsByPloIdAsync(Guid ploId)
        {
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) &
                         Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var projection = Builders<CLO_PLO_Mapping>.Projection.Expression(m => m.CloId);
            return await _collection.Find(filter).Project(projection).ToListAsync();
        }

        public async Task<List<(Guid Id, string Details)>> GetCLOsWithDetailsByPloIdAsync(Guid ploId)
        {
            // Step 1: Get CLO IDs from mappings
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) &
                         Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var mappings = await _collection.Find(filter).ToListAsync();

            if (!mappings.Any())
            {
                return new List<(Guid, string)>();
            }
            var cloIds = mappings.Select(m => m.CloId).ToList();

            // Step 2: Get CLO details from CLOs collection
            var cloCollection = _collection.Database.GetCollection<CLOs>("CLOs");
            var cloFilter = Builders<CLOs>.Filter.In(c => c.Id, cloIds) &
                           Builders<CLOs>.Filter.Eq(c => c.DelFlg, false);

            var clos = await cloCollection.Find(cloFilter).ToListAsync();

            // Step 3: Map to result format
            return clos.Select(c => (c.Id, c.CloDetails)).ToList();
        }
        public async Task UpdateAsync(CLO_PLO_Mapping mapping)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(mapping));
            }

            mapping.UpdDate = DateTime.UtcNow;

            // Create a filter to find the document by ID
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.Id, mapping.Id);

            // Replace the existing document with the updated one
            await _collection.ReplaceOneAsync(filter, mapping);
        }

        public async Task<CLO_PLO_Mapping?> GetMappingAsync(Guid cloId, Guid ploId)
        {
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.CloId, cloId) &
                         Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) &
                         NotDeletedFilter;

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateManyAsync(IEnumerable<CLO_PLO_Mapping> mappings)
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
        public async Task AddManyAsync(IEnumerable<CLO_PLO_Mapping> mappings)
        {
            if (mappings == null)
            {
                throw new ArgumentNullException(nameof(mappings));
            }

            await InsertManyAsync(mappings);
        }
        public async Task<List<(Guid Id, string PloName, string CurriculumName, string Description)>> GetPLOsWithDetailsByCloIdAsync(Guid cloId)
        {
            // Step 1: Get PLO IDs from mappings
            var filter = Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.CloId, cloId) &
                         Builders<CLO_PLO_Mapping>.Filter.Eq(m => m.DelFlg, false);
            var mappings = await _collection.Find(filter).ToListAsync();

            if (!mappings.Any())
            {
                return new List<(Guid, string, string, string)>();
            }
            var ploIds = mappings.Select(m => m.PloId).ToList();

            // Step 2: Get PLO details from PLOs collection
            var ploCollection = _collection.Database.GetCollection<PLOs>("PLOs");
            var ploFilter = Builders<PLOs>.Filter.In(p => p.Id, ploIds) &
                           Builders<PLOs>.Filter.Eq(p => p.DelFlg, false);

            var plos = await ploCollection.Find(ploFilter).ToListAsync();

            // Step 3: Get curriculum information for each PLO
            var curriculumIds = plos.Select(p => p.CurriculumId).Distinct().ToList();
            var curriculumCollection = _collection.Database.GetCollection<Curriculums>("Curriculums");
            var curriculumFilter = Builders<Curriculums>.Filter.In(c => c.Id, curriculumIds) &
                                  Builders<Curriculums>.Filter.Eq(c => c.DelFlg, false);

            var curriculums = await curriculumCollection.Find(curriculumFilter).ToListAsync();
            var curriculumDict = curriculums.ToDictionary(c => c.Id, c => c.CurriculumName);

            // Step 4: Map to result format
            return plos.Select(p => (
                p.Id,
                p.PloName,
                curriculumDict.TryGetValue(p.CurriculumId, out var name) ? name : string.Empty,
                p.Description
            )).ToList();
        }
    }
}

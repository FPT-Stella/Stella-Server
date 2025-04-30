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
        /// <summary>
        /// Updates a PO_PLO mapping entity in the database.
        /// </summary>
        /// <param name="mapping">The mapping entity to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task UpdateAsync(PO_PLO_Mapping mapping)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(mapping));
            }

            mapping.UpdDate = DateTime.UtcNow;

            // Create a filter to find the document by ID
            var filter = Builders<PO_PLO_Mapping>.Filter.Eq(m => m.Id, mapping.Id);

            // Replace the existing document with the updated one
            await _collection.ReplaceOneAsync(filter, mapping);
        }
        /// <summary>
        /// Gets a specific mapping between PO and PLO.
        /// </summary>
        /// <param name="poId">The PO ID</param>
        /// <param name="ploId">The PLO ID</param>
        /// <returns>The mapping if it exists, otherwise null</returns>
        public async Task<PO_PLO_Mapping?> GetMappingAsync(Guid poId, Guid ploId)
        {
            var filter = Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PoId, poId) &
                         Builders<PO_PLO_Mapping>.Filter.Eq(m => m.PloId, ploId) &
                         NotDeletedFilter;

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
        /// <summary>
        /// Updates multiple PO_PLO mappings in the database.
        /// </summary>
        /// <param name="mappings">The collection of mapping entities to update</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task UpdateManyAsync(IEnumerable<PO_PLO_Mapping> mappings)
        {
            if (mappings == null)
            {
                throw new ArgumentNullException(nameof(mappings));
            }

            // Process each mapping individually using the existing UpdateAsync method
            foreach (var mapping in mappings)
            {
                await UpdateAsync(mapping);
            }
        }
        /// <summary>
        /// Adds multiple PO_PLO mappings to the database in a single operation.
        /// </summary>
        /// <param name="mappings">The collection of mappings to add</param>
        /// <returns>A task representing the asynchronous operation</returns>
        public async Task AddManyAsync(IEnumerable<PO_PLO_Mapping> mappings)
        {
            if (mappings == null)
            {
                throw new ArgumentNullException(nameof(mappings));
            }

            // Use the existing InsertManyAsync method for bulk insertion
            await InsertManyAsync(mappings);
        }
    }
}

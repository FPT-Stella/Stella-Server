using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Domain.Entities;
using FPTStella.Infrastructure.UnitOfWorks.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.Data
{
    public class SubjectToolRepository : Repository<SubjectTool>, ISubjectToolRepository
    {
        private static readonly FilterDefinition<SubjectTool> NotDeletedFilter =
            Builders<SubjectTool>.Filter.Eq(m => m.DelFlg, false);

        public SubjectToolRepository(IMongoDatabase database) : base(database, "SubjectTool")
        {
        }

        public async Task<List<SubjectTool>> GetBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.SubjectId, subjectId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<SubjectTool>> GetByToolIdAsync(Guid toolId)
        {
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.ToolId, toolId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> IsMappingExistedAsync(Guid subjectId, Guid toolId)
        {
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.SubjectId, subjectId) &
                         Builders<SubjectTool>.Filter.Eq(m => m.ToolId, toolId) &
                         Builders<SubjectTool>.Filter.Eq(m => m.DelFlg, false);
            return await _collection.CountDocumentsAsync(filter) > 0;
        }

        public async Task DeleteMappingsBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.SubjectId, subjectId) &
                         Builders<SubjectTool>.Filter.Eq(m => m.DelFlg, false);
            var update = Builders<SubjectTool>.Update.Set(m => m.DelFlg, true)
                                                   .Set(m => m.UpdDate, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update);
        }

        public async Task DeleteMappingsByToolIdAsync(Guid toolId)
        {
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.ToolId, toolId) &
                         Builders<SubjectTool>.Filter.Eq(m => m.DelFlg, false);
            var update = Builders<SubjectTool>.Update.Set(m => m.DelFlg, true)
                                                   .Set(m => m.UpdDate, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update);
        }

        public async Task<List<Guid>> GetToolIdsBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.SubjectId, subjectId) &
                         Builders<SubjectTool>.Filter.Eq(m => m.DelFlg, false);
            var projection = Builders<SubjectTool>.Projection.Expression(m => m.ToolId);
            return await _collection.Find(filter).Project(projection).ToListAsync();
        }

        public async Task<List<Guid>> GetSubjectIdsByToolIdAsync(Guid toolId)
        {
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.ToolId, toolId) &
                         Builders<SubjectTool>.Filter.Eq(m => m.DelFlg, false);
            var projection = Builders<SubjectTool>.Projection.Expression(m => m.SubjectId);
            return await _collection.Find(filter).Project(projection).ToListAsync();
        }

        public async Task<List<(Guid Id, string Name)>> GetToolsWithNameBySubjectIdAsync(Guid subjectId)
        {
            // Step 1: Get Tool IDs from mappings
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.SubjectId, subjectId) &
                         Builders<SubjectTool>.Filter.Eq(m => m.DelFlg, false);
            var mappings = await _collection.Find(filter).ToListAsync();

            if (!mappings.Any())
            {
                return new List<(Guid, string)>();
            }

            var toolIds = mappings.Select(m => m.ToolId).ToList();

            // Step 2: Get Tool details from Tools collection
            var toolCollection = _collection.Database.GetCollection<Tools>("Tools");
            var toolFilter = Builders<Tools>.Filter.In(p => p.Id, toolIds) &
                           Builders<Tools>.Filter.Eq(p => p.DelFlg, false);

            var tools = await toolCollection.Find(toolFilter).ToListAsync();

            // Step 3: Map to result format
            return tools.Select(t => (t.Id, t.ToolName)).ToList();
        }

        public async Task<List<(Guid Id, string Name)>> GetSubjectsWithNameByToolIdAsync(Guid toolId)
        {
            // Step 1: Get Subject IDs from mappings
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.ToolId, toolId) &
                         Builders<SubjectTool>.Filter.Eq(m => m.DelFlg, false);
            var mappings = await _collection.Find(filter).ToListAsync();

            if (!mappings.Any())
            {
                return new List<(Guid, string)>();
            }

            var subjectIds = mappings.Select(m => m.SubjectId).ToList();

            // Step 2: Get Subject details from Subjects collection
            var subjectCollection = _collection.Database.GetCollection<Subjects>("Subjects");
            var subjectFilter = Builders<Subjects>.Filter.In(s => s.Id, subjectIds) &
                              Builders<Subjects>.Filter.Eq(s => s.DelFlg, false);

            var subjects = await subjectCollection.Find(subjectFilter).ToListAsync();

            // Step 3: Map to result format
            return subjects.Select(s => (s.Id, s.SubjectName)).ToList();
        }

        public async Task UpdateAsync(SubjectTool mapping)
        {
            if (mapping == null)
            {
                throw new ArgumentNullException(nameof(mapping));
            }

            mapping.UpdDate = DateTime.UtcNow;

            // Create a filter to find the document by ID
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.Id, mapping.Id);

            // Replace the existing document with the updated one
            await _collection.ReplaceOneAsync(filter, mapping);
        }

        public async Task<SubjectTool?> GetMappingAsync(Guid subjectId, Guid toolId)
        {
            var filter = Builders<SubjectTool>.Filter.Eq(m => m.SubjectId, subjectId) &
                         Builders<SubjectTool>.Filter.Eq(m => m.ToolId, toolId) &
                         NotDeletedFilter;

            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task UpdateManyAsync(IEnumerable<SubjectTool> mappings)
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

        public async Task AddManyAsync(IEnumerable<SubjectTool> mappings)
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
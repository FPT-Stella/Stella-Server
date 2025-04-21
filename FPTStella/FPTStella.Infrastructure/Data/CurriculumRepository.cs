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
    public class CurriculumRepository : Repository<Curriculums>, ICurriculumRepository
    {
        private static readonly FilterDefinition<Curriculums> NotDeletedFilter =
            Builders<Curriculums>.Filter.Eq(c => c.DelFlg, false);
        public CurriculumRepository(IMongoDatabase database) : base(database, "Curriculums")
        {
           
             var curriculumCodeIndex = new CreateIndexModel<Curriculums>(
                Builders<Curriculums>.IndexKeys.Ascending(c => c.CurriculumCode),
                new CreateIndexOptions { Unique = true }
             );

            var programIdIndex = new CreateIndexModel<Curriculums>(
                Builders<Curriculums>.IndexKeys.Ascending(c => c.ProgramId),
                new CreateIndexOptions
                {
                    Unique = false,
                    Background = true 
                }
            );

            var curriculumNameIndex = new CreateIndexModel<Curriculums>(
                Builders<Curriculums>.IndexKeys.Ascending(c => c.CurriculumName),
                new CreateIndexOptions { Background = true }
            );
            Collection.Indexes.CreateMany(new[] { curriculumCodeIndex, programIdIndex });
        }
        public async Task<Curriculums?> GetByCurriculumCodeAsync(string curriculumCode)
        {
            var filter = Builders<Curriculums>.Filter.Eq(c => c.CurriculumCode, curriculumCode) & NotDeletedFilter;
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<List<Curriculums>> GetByProgramIdAsync(Guid programId)
        {
            var filter = Builders<Curriculums>.Filter.Eq(c => c.ProgramId, programId) & NotDeletedFilter;
            return await Collection.Find(filter).ToListAsync();
        }
        public async Task<Curriculums?> GetByCurriculumNameAsync(string curriculumName)
        {
            var filter = Builders<Curriculums>.Filter.Eq(c => c.CurriculumName, curriculumName) & NotDeletedFilter;
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<bool> IsCurriculumCodeExisted(string curriculumCode)
        {
            var filter = Builders<Curriculums>.Filter.Eq(c => c.CurriculumCode, curriculumCode) & NotDeletedFilter;
            return await Collection.CountDocumentsAsync(filter) > 0;
        }
        public async Task<bool> IsCurriculumNameExisted(string curriculumName)
        {
            var filter = Builders<Curriculums>.Filter.Eq(c => c.CurriculumName, curriculumName) & NotDeletedFilter;
            return await Collection.CountDocumentsAsync(filter) > 0;
        }
        public async Task<bool> IsCurriculumCodeExisted(string curriculumCode, Guid id)
        {
            var filter = Builders<Curriculums>.Filter.Eq(c => c.CurriculumCode, curriculumCode)
                         & NotDeletedFilter
                         & Builders<Curriculums>.Filter.Ne(c => c.Id, id);
            return await Collection.CountDocumentsAsync(filter) > 0;
        }
        public async Task<bool> IsCurriculumNameExisted(string curriculumName, Guid id)
        {
            var filter = Builders<Curriculums>.Filter.Eq(c => c.CurriculumName, curriculumName)
                         & NotDeletedFilter
                         & Builders<Curriculums>.Filter.Ne(c => c.Id, id);
            return await Collection.CountDocumentsAsync(filter) > 0;
        }
    }
}

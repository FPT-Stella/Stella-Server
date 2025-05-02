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
    public class CLORepository : Repository<CLOs>, ICLORepository
    {
        private static readonly FilterDefinition<CLOs> NotDeletedFilter =
            Builders<CLOs>.Filter.Eq(c => c.DelFlg, false);

        public CLORepository(IMongoDatabase database) : base(database, "CLOs")
        {
        }

        public async Task<List<CLOs>> GetBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<CLOs>.Filter.Eq(c => c.SubjectId, subjectId) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<bool> IsCloExistedAsync(Guid subjectId, string cloDetails)
        {
            var filter = Builders<CLOs>.Filter.Eq(c => c.SubjectId, subjectId) &
                         Builders<CLOs>.Filter.Eq(c => c.CloDetails, cloDetails) &
                         NotDeletedFilter;
            return await _collection.CountDocumentsAsync(filter) > 0;
        }

        public async Task DeleteClosBySubjectIdAsync(Guid subjectId)
        {
            var filter = Builders<CLOs>.Filter.Eq(c => c.SubjectId, subjectId) &
                         NotDeletedFilter;
            var update = Builders<CLOs>.Update.Set(c => c.DelFlg, true)
                                             .Set(c => c.UpdDate, DateTime.UtcNow);
            await _collection.UpdateManyAsync(filter, update);
        }
    }
}

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
    public class ProgramRepository : Repository<Programs>, IProgramRepository
    {
        public ProgramRepository(IMongoDatabase database) : base(database, "Programs")
        {
            // Tạo index unique cho program_code
            var indexKeys = Builders<Programs>.IndexKeys.Ascending("program_code");
            var indexOptions = new CreateIndexOptions { Unique = true };
            Collection.Indexes.CreateOne(new CreateIndexModel<Programs>(indexKeys, indexOptions));
            // Tạo index cho major_id
            var majorIdIndexKeys = Builders<Programs>.IndexKeys.Ascending("major_id");
            Collection.Indexes.CreateOne(new CreateIndexModel<Programs>(majorIdIndexKeys));
        }
        public async Task<Programs?> GetByProgramCodeAsync(string programCode)
        {
            var filter = Builders<Programs>.Filter.Eq("program_code", programCode) & Builders<Programs>.Filter.Eq("del_flg", false);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<Programs?> GetByMajorIdAsync(Guid majorId)
        {
            var filter = Builders<Programs>.Filter.Eq("major_id", majorId) & Builders<Programs>.Filter.Eq("del_flg", false);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<Programs?> GetByProgramNameAsync(string programName)
        {
            var filter = Builders<Programs>.Filter.Eq("program_name", programName) & Builders<Programs>.Filter.Eq("del_flg", false);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<Programs?> GetByMajorIdAndProgramNameAsync(Guid majorId, string programName)
        {
            var filter = Builders<Programs>.Filter.Eq("major_id", majorId) & Builders<Programs>.Filter.Eq("program_name", programName) & Builders<Programs>.Filter.Eq("del_flg", false);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<Programs?> GetByMajorIdAndProgramCodeAsync(Guid majorId, string programCode)
        {
            var filter = Builders<Programs>.Filter.Eq("major_id", majorId) & Builders<Programs>.Filter.Eq("program_code", programCode) & Builders<Programs>.Filter.Eq("del_flg", false);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}

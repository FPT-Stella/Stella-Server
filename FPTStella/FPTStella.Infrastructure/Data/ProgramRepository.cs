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
            // Index duy nhất cho program_code
            Collection.Indexes.CreateOne(new CreateIndexModel<Programs>(
                Builders<Programs>.IndexKeys.Ascending(p => p.ProgramCode),
                new CreateIndexOptions { Unique = true }));

            // Index tìm theo major_id
            Collection.Indexes.CreateOne(new CreateIndexModel<Programs>(
                Builders<Programs>.IndexKeys.Ascending(p => p.MajorId)));
        }
        public async Task<Programs?> GetByProgramCodeAsync(string programCode)
        {
            return await FindOneAsync(p => p.ProgramCode == programCode && p.DelFlg == false);
        }

        public async Task<Programs?> GetByMajorIdAsync(Guid majorId)
        {
            return await FindOneAsync(p => p.MajorId == majorId && p.DelFlg == false);
        }

        public async Task<Programs?> GetByProgramNameAsync(string programName)
        {
            return await FindOneAsync(p => p.ProgramName == programName && p.DelFlg == false);
        }

        public async Task<Programs?> GetByMajorIdAndProgramNameAsync(Guid majorId, string programName)
        {
            return await FindOneAsync(p => p.MajorId == majorId && p.ProgramName == programName && p.DelFlg == false);
        }

        public async Task<Programs?> GetByMajorIdAndProgramCodeAsync(Guid majorId, string programCode)
        {
            return await FindOneAsync(p => p.MajorId == majorId && p.ProgramCode == programCode && p.DelFlg == false);
        }
    }
}

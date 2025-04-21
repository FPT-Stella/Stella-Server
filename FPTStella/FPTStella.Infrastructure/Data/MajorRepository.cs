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
    public class MajorRepository : Repository<Majors>, IMajorRepository
    {
        public MajorRepository(IMongoDatabase database) : base(database, "Majors")
        {
            // Tạo index unique cho major_name
            var indexKeys = Builders<Majors>.IndexKeys.Ascending("major_name");
            var indexOptions = new CreateIndexOptions { Unique = true };
            Collection.Indexes.CreateOne(new CreateIndexModel<Majors>(indexKeys, indexOptions));
        }
        public async Task<Majors?> GetByMajorNameAsync(string majorName)
        {
            var filter = Builders<Majors>.Filter.Eq("major_name", majorName) & Builders<Majors>.Filter.Eq("del_flg", false);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}

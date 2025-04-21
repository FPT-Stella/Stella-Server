using FPTStella.Application.Common.Interfaces.Persistences;
using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
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
    public class StudentRepository : Repository<Student>, IStudentRepository
    {
        public StudentRepository(IMongoDatabase database) : base(database, "Students")
        {
            // Tạo index unique cho student_code
            var indexKeys = Builders<Student>.IndexKeys.Ascending("student_code");
            var indexOptions = new CreateIndexOptions { Unique = true };
            Collection.Indexes.CreateOne(new CreateIndexModel<Student>(indexKeys, indexOptions));

            // Tạo index cho user_id
            var userIdIndexKeys = Builders<Student>.IndexKeys.Ascending("user_id");
            Collection.Indexes.CreateOne(new CreateIndexModel<Student>(userIdIndexKeys));

            // Tạo index cho major_id
            var majorIdIndexKeys = Builders<Student>.IndexKeys.Ascending("major_id");
            Collection.Indexes.CreateOne(new CreateIndexModel<Student>(majorIdIndexKeys));
        }

        public async Task<Student?> GetByStudentCodeAsync(string studentCode)
        {
            var filter = Builders<Student>.Filter.Eq("student_code", studentCode) & Builders<Student>.Filter.Eq("del_flg", false);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Student?> GetByUserIdAsync(Guid userId)
        {
            var filter = Builders<Student>.Filter.Eq("user_id", userId) & Builders<Student>.Filter.Eq("del_flg", false);
            return await Collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}

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
            // Index duy nhất cho student_code
            _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                Builders<Student>.IndexKeys.Ascending(s => s.StudentCode),
                new CreateIndexOptions { Unique = true }));

            // Index cho user_id
            _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                Builders<Student>.IndexKeys.Ascending(s => s.UserId)));

            // Index cho major_id
            _collection.Indexes.CreateOne(new CreateIndexModel<Student>(
                Builders<Student>.IndexKeys.Ascending(s => s.MajorId)));
        }

        public async Task<Student?> GetByStudentCodeAsync(string studentCode)
        {
            return await FindOneAsync(s => s.StudentCode == studentCode && s.DelFlg == false);
        }

        public async Task<Student?> GetByUserIdAsync(Guid userId)
        {
            return await FindOneAsync(s => s.UserId == userId && s.DelFlg == false);
        }
    }
}

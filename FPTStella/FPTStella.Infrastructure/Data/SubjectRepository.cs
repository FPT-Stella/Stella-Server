using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Domain.Entities;
using FPTStella.Infrastructure.UnitOfWorks.Repositories;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FPTStella.Infrastructure.Data
{
    public class SubjectRepository : Repository<Subjects>, ISubjectRepository
    {
        private static readonly FilterDefinition<Subjects> NotDeletedFilter =
            Builders<Subjects>.Filter.Eq(s => s.DelFlg, false);

        public SubjectRepository(IMongoDatabase database) : base(database, "Subjects")
        {
            var subjectCodeIndex = new CreateIndexModel<Subjects>(
                Builders<Subjects>.IndexKeys.Ascending(s => s.SubjectCode),
                new CreateIndexOptions { Unique = true });

            var subjectNameIndex = new CreateIndexModel<Subjects>(
                Builders<Subjects>.IndexKeys.Ascending(s => s.SubjectName),
                new CreateIndexOptions { Background = true });

            var termNoIndex = new CreateIndexModel<Subjects>(
                Builders<Subjects>.IndexKeys.Ascending(s => s.TermNo),
                new CreateIndexOptions { Background = true });

            _collection.Indexes.CreateMany(new[] { subjectCodeIndex, subjectNameIndex, termNoIndex });
        }

        public async Task<List<Subjects>> GetAllSubjectsAsync()
        {
            return await _collection.Find(NotDeletedFilter).ToListAsync();
        }

        public async Task<Subjects?> GetSubjectByIdAsync(Guid id)
        {
            var filter = Builders<Subjects>.Filter.Eq(s => s.Id, id) & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<Subjects>> GetSubjectsByDegreeLevelAsync(string degreeLevel)
        {
            var filter = Builders<Subjects>.Filter.Eq(s => s.DegreeLevel, degreeLevel) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task<List<Subjects>> GetSubjectsByTermNoAsync(int termNo)
        {
            var filter = Builders<Subjects>.Filter.Eq(s => s.TermNo, termNo) & NotDeletedFilter;
            return await _collection.Find(filter).ToListAsync();
        }

        public async Task CreateSubjectAsync(Subjects subject)
        {
            await _collection.InsertOneAsync(subject);
        }
        public async Task<Subjects?> GetBySubjectCodeAsync(string subjectCode)
        {
            var filter = Builders<Subjects>.Filter.Eq(s => s.SubjectCode, subjectCode) & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Subjects?> GetBySubjectNameAsync(string subjectName)
        {
            var filter = Builders<Subjects>.Filter.Eq(s => s.SubjectName, subjectName) & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Subjects?> GetBySubjectCodeAndSubjectNameAsync(string subjectCode, string subjectName)
        {
            var filter = Builders<Subjects>.Filter.Eq(s => s.SubjectCode, subjectCode)
                         & Builders<Subjects>.Filter.Eq(s => s.SubjectName, subjectName)
                         & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
        public async Task<Subjects?> GetByMajorIdAndSubjectCodeAsync(Guid majorId, string subjectCode)
        {
            // Modified to use the PrerequisiteName for major reference
            var filter = Builders<Subjects>.Filter.Eq(s => s.SubjectCode, subjectCode)
                         & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<Subjects?> GetByMajorIdAndSubjectNameAsync(Guid majorId, string subjectName)
        {
            // Modified to use the PrerequisiteName for major reference
            var filter = Builders<Subjects>.Filter.Eq(s => s.SubjectName, subjectName)
                         & NotDeletedFilter;
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}

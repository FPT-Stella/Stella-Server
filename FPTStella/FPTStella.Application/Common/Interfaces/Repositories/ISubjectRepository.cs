using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface ISubjectRepository : IRepository<Subjects>
    {
        Task<List<Subjects>> GetAllSubjectsAsync();
        Task<Subjects?> GetSubjectByIdAsync(Guid id);
        Task<List<Subjects>> GetSubjectsByDegreeLevelAsync(string degreeLevel);
        Task<List<Subjects>> GetSubjectsByTermNoAsync(int termNo);
        Task CreateSubjectAsync(Subjects subject);
        Task<Subjects?> GetBySubjectCodeAsync(string subjectCode);
        Task<Subjects?> GetBySubjectNameAsync(string subjectName);
        Task<Subjects?> GetBySubjectCodeAndSubjectNameAsync(string subjectCode, string subjectName);
        Task<Subjects?> GetByMajorIdAndSubjectCodeAsync(Guid majorId, string subjectCode);
        Task<Subjects?> GetByMajorIdAndSubjectNameAsync(Guid majorId, string subjectName);
    }
}

using FPTStella.Contracts.DTOs.Subjects;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface ISubjectService
    {
        Task<List<Subjects>> GetAllSubjectsAsync();
        Task<Subjects> GetSubjectByIdAsync(string id);
        Task<List<Subjects>> GetSubjectsByDegreeLevelAsync(string degreeLevel);
        Task CreateSubjectAsync(CreateSubjectDto subject);
        Task<Subjects?> GetBySubjectCodeAsync(string subjectCode);
        Task<Subjects?> GetBySubjectNameAsync(string subjectName);
        Task<Subjects?> GetBySubjectCodeAndSubjectNameAsync(string subjectCode, string subjectName);
        Task<Subjects?> GetByMajorIdAndSubjectCodeAsync(Guid majorId, string subjectCode);
        Task<Subjects?> GetByMajorIdAndSubjectNameAsync(Guid majorId, string subjectName);
        Task<bool> UpdateSubjectAsync(string id, UpdateSubjectDto updatedSubject);
        Task<bool> DeleteSubjectAsync(string id);
    }
}

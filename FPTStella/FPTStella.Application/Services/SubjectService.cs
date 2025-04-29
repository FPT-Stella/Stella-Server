using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.Subjects;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(IUnitOfWork unitOfWork, ISubjectRepository subjectRepository)
        {
            _unitOfWork = unitOfWork;
            _subjectRepository = subjectRepository;
        }

        private static SubjectDto MapToSubjectDto(Subjects subject)
        {
            return new SubjectDto
            {
                Id = subject.Id.ToString(),
                SubjectCode = subject.SubjectCode,
                SubjectName = subject.SubjectName,
                SubjectDescription = subject.SubjectDescription,
                Credits = subject.Credits,
                Prerequisite = subject.Prerequisite,
                PrerequisiteName = subject.PrerequisiteName,
                DegreeLevel = subject.DegreeLevel,
                TimeAllocation = subject.TimeAllocation,
                SysllabusDescription = subject.SysllabusDescription,
                StudentTask = subject.StudentTask,
                ScoringScale = subject.ScoringScale,
                MinAvgMarkToPass = subject.MinAvgMarkToPass,
                Note = subject.Note,
                Topic = subject.Topic,
                LearningTeachingType = subject.LearningTeachingType,
                TermNo = subject.TermNo
            };
        }

        public async Task<List<Subjects>> GetAllSubjectsAsync()
        {
            return await _subjectRepository.GetAllSubjectsAsync();
        }

        public async Task<Subjects> GetSubjectByIdAsync(string id)
        {
            var repo = _unitOfWork.Repository<Subjects>();
            var subject = await repo.GetByIdAsync(id);
            if (subject == null || subject.DelFlg)
                throw new KeyNotFoundException("Subject not found.");

            return subject;
        }

        public async Task<List<Subjects>> GetSubjectsByDegreeLevelAsync(string degreeLevel)
        {
            return await _subjectRepository.GetSubjectsByDegreeLevelAsync(degreeLevel);
        }

        public async Task<SubjectDto> CreateSubjectAsync(CreateSubjectDto subject)
        {
            var repo = _unitOfWork.Repository<Subjects>();

            if (await repo.FindOneAsync(s => s.SubjectCode == subject.SubjectCode) != null)
                throw new InvalidOperationException("SubjectCode already exists.");

            if (await repo.FindOneAsync(s => s.SubjectName == subject.SubjectName) != null)
                throw new InvalidOperationException("SubjectName already exists.");

            var subjectEntity = new Subjects
            {
                SubjectName = subject.SubjectName,
                SubjectCode = subject.SubjectCode,
                SubjectDescription = subject.SubjectDescription,
                Credits = subject.Credits,
                Prerequisite = subject.Prerequisite,
                PrerequisiteName = subject.PrerequisiteName,
                DegreeLevel = subject.DegreeLevel,
                TimeAllocation = subject.TimeAllocation,
                SysllabusDescription = subject.SysllabusDescription,
                StudentTask = subject.StudentTask,
                ScoringScale = subject.ScoringScale,
                MinAvgMarkToPass = subject.MinAvgMarkToPass,
                Note = subject.Note,
                Topic = subject.Topic,
                LearningTeachingType = subject.LearningTeachingType,
                TermNo = subject.TermNo
            };

            await repo.InsertAsync(subjectEntity);
            await _unitOfWork.SaveAsync();
            return MapToSubjectDto(subjectEntity);
        }

        public async Task<Subjects?> GetBySubjectCodeAsync(string subjectCode)
        {
            return await _subjectRepository.GetBySubjectCodeAsync(subjectCode);
        }

        public async Task<Subjects?> GetBySubjectNameAsync(string subjectName)
        {
            return await _subjectRepository.GetBySubjectNameAsync(subjectName);
        }

        public async Task<Subjects?> GetBySubjectCodeAndSubjectNameAsync(string subjectCode, string subjectName)
        {
            return await _subjectRepository.GetBySubjectCodeAndSubjectNameAsync(subjectCode, subjectName);
        }

        public async Task<Subjects?> GetByMajorIdAndSubjectCodeAsync(Guid majorId, string subjectCode)
        {
            return await _subjectRepository.GetByMajorIdAndSubjectCodeAsync(majorId, subjectCode);
        }

        public async Task<Subjects?> GetByMajorIdAndSubjectNameAsync(Guid majorId, string subjectName)
        {
            return await _subjectRepository.GetByMajorIdAndSubjectNameAsync(majorId, subjectName);
        }
        public async Task<List<Subjects>> GetSubjectsByTermNoAsync(int termNo)
        {
            return await _subjectRepository.GetSubjectsByTermNoAsync(termNo);
        }
        public async Task<bool> UpdateSubjectAsync(string id, UpdateSubjectDto updatedSubject)
        {
            var repo = _unitOfWork.Repository<Subjects>();
            var existing = await repo.GetByIdAsync(id);

            if (existing == null || existing.DelFlg)
                throw new KeyNotFoundException("Subject not found.");

            existing.SubjectName = !string.IsNullOrWhiteSpace(updatedSubject.SubjectName)
                ? updatedSubject.SubjectName : existing.SubjectName;

            existing.SubjectCode = !string.IsNullOrWhiteSpace(updatedSubject.SubjectCode)
                ? updatedSubject.SubjectCode : existing.SubjectCode;

            existing.SubjectDescription = !string.IsNullOrWhiteSpace(updatedSubject.SubjectDescription)
                ? updatedSubject.SubjectDescription : existing.SubjectDescription;

            existing.Credits = updatedSubject.Credits != 0
                ? updatedSubject.Credits : existing.Credits;

            existing.Prerequisite = updatedSubject.Prerequisite;

            existing.PrerequisiteName = !string.IsNullOrWhiteSpace(updatedSubject.PrerequisiteName)
                ? updatedSubject.PrerequisiteName : existing.PrerequisiteName;

            existing.DegreeLevel = !string.IsNullOrWhiteSpace(updatedSubject.DegreeLevel)
                ? updatedSubject.DegreeLevel : existing.DegreeLevel;

            existing.TimeAllocation = !string.IsNullOrWhiteSpace(updatedSubject.TimeAllocation)
                ? updatedSubject.TimeAllocation : existing.TimeAllocation;

            existing.SysllabusDescription = !string.IsNullOrWhiteSpace(updatedSubject.SysllabusDescription)
                ? updatedSubject.SysllabusDescription : existing.SysllabusDescription;

            existing.StudentTask = !string.IsNullOrWhiteSpace(updatedSubject.StudentTask)
                ? updatedSubject.StudentTask : existing.StudentTask;

            existing.ScoringScale = updatedSubject.ScoringScale != 0
                ? updatedSubject.ScoringScale : existing.ScoringScale;

            existing.MinAvgMarkToPass = updatedSubject.MinAvgMarkToPass != 0
                ? updatedSubject.MinAvgMarkToPass : existing.MinAvgMarkToPass;

            existing.Note = !string.IsNullOrWhiteSpace(updatedSubject.Note)
                ? updatedSubject.Note : existing.Note;
            existing.Topic = !string.IsNullOrWhiteSpace(updatedSubject.Topic)
                ? updatedSubject.Topic : existing.Topic;

            existing.LearningTeachingType = updatedSubject.LearningTeachingType;

            existing.TermNo = updatedSubject.TermNo != 0
                ? updatedSubject.TermNo : existing.TermNo;

            existing.UpdDate = DateTime.UtcNow;

            await repo.ReplaceAsync(id, existing);
            await _unitOfWork.SaveAsync();

            return true;
        }
        public async Task<bool> DeleteSubjectAsync(string id)
        {
            var repo = _unitOfWork.Repository<Subjects>();
            var subject = await repo.GetByIdAsync(id);

            if (subject == null || subject.DelFlg)
                throw new KeyNotFoundException("Subject not found.");

            subject.DelFlg = true;
            subject.UpdDate = DateTime.UtcNow;

            await repo.ReplaceAsync(id, subject);
            await _unitOfWork.SaveAsync();

            return true;
        }
    }
}

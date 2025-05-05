using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IStudentRepository _studentRepository;
        private readonly ISubjectRepository _subjectRepository;
        private readonly IMajorRepository _majorRepository;
        private readonly IProgramRepository _programRepository;
        private readonly ICurriculumRepository _curriculumRepository;
        private readonly ICLORepository _cloRepository;
        private readonly IPLORepository _ploRepository;

        public DashboardService(
            IStudentRepository studentRepository,
            ISubjectRepository subjectRepository,
            IMajorRepository majorRepository,
            IProgramRepository programRepository,
            ICurriculumRepository curriculumRepository,
            ICLORepository cloRepository,
            IPLORepository ploRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
            _subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));
            _majorRepository = majorRepository ?? throw new ArgumentNullException(nameof(majorRepository));
            _programRepository = programRepository ?? throw new ArgumentNullException(nameof(programRepository));
            _curriculumRepository = curriculumRepository ?? throw new ArgumentNullException(nameof(curriculumRepository));
            _cloRepository = cloRepository ?? throw new ArgumentNullException(nameof(cloRepository));
            _ploRepository = ploRepository ?? throw new ArgumentNullException(nameof(ploRepository));
        }

        /// <summary>
        /// Gets the general dashboard statistics.
        /// </summary>
        /// <returns>The general dashboard statistics.</returns>
        public async Task<DashboardStatisticsDto> GetDashboardStatisticsAsync()
        {
            // Count students
            var students = await _studentRepository.FilterByAsync(s => !s.DelFlg);
            int totalStudents = students.Count();

            // Count subjects
            var subjects = await _subjectRepository.FilterByAsync(s => !s.DelFlg);
            int totalSubjects = subjects.Count();

            // Count programs
            var programs = await _programRepository.FilterByAsync(p => !p.DelFlg);
            int totalPrograms = programs.Count();

            // Count majors
            var majors = await _majorRepository.FilterByAsync(m => !m.DelFlg);
            int totalMajors = majors.Count();

            // Count curriculums
            var curriculums = await _curriculumRepository.FilterByAsync(c => !c.DelFlg);
            int totalCurriculums = curriculums.Count();

            // Count CLOs
            var clos = await _cloRepository.FilterByAsync(c => !c.DelFlg);
            int totalCLOs = clos.Count();

            // Count PLOs
            var plos = await _ploRepository.FilterByAsync(p => !p.DelFlg);
            int totalPLOs = plos.Count();

            // Create result
            var result = new DashboardStatisticsDto
            {
                TotalStudents = totalStudents,
                TotalSubjects = totalSubjects,
                TotalPrograms = totalPrograms,
                TotalMajors = totalMajors,
                TotalCurriculums = totalCurriculums,
                TotalCLOs = totalCLOs,
                TotalPLOs = totalPLOs
            };

            return result;
        }

        /// <summary>
        /// Gets statistics about students grouped by major.
        /// </summary>
        /// <returns>Student statistics by major.</returns>
        public async Task<StudentMajorStatisticsDto> GetStudentMajorStatisticsAsync()
        {
            // Get all active students
            var students = await _studentRepository.FilterByAsync(s => !s.DelFlg);
            var studentsList = students.ToList();
            int totalStudents = studentsList.Count;

            // Get all active majors
            var majors = await _majorRepository.FilterByAsync(m => !m.DelFlg);
            var majorsList = majors.ToList();

            // Calculate students by major
            var studentsByMajor = new List<MajorStatistics>();
            foreach (var major in majorsList)
            {
                var studentsInMajor = studentsList.Count(s => s.MajorId == major.Id);
                studentsByMajor.Add(new MajorStatistics
                {
                    MajorId = major.Id.ToString(),
                    MajorName = major.MajorName,
                    StudentCount = studentsInMajor
                });
            }

            // Create result
            var result = new StudentMajorStatisticsDto
            {
                TotalStudents = totalStudents,
                StudentsByMajor = studentsByMajor.OrderByDescending(m => m.StudentCount).ToList()
            };

            return result;
        }

        /// <summary>
        /// Gets statistics about subjects grouped by credits.
        /// </summary>
        /// <returns>Subject statistics by credits.</returns>
        public async Task<SubjectCreditStatisticsDto> GetSubjectCreditStatisticsAsync()
        {
            // Get all active subjects
            var subjects = await _subjectRepository.FilterByAsync(s => !s.DelFlg);
            var subjectsList = subjects.ToList();
            int totalSubjects = subjectsList.Count;

            // Calculate subjects by credits
            var subjectsByCredits = subjectsList
                .GroupBy(s => s.Credits)
                .Select(g => new CreditStatistics
                {
                    Credits = g.Key,
                    SubjectCount = g.Count()
                })
                .OrderBy(c => c.Credits)
                .ToList();

            // Create result
            var result = new SubjectCreditStatisticsDto
            {
                TotalSubjects = totalSubjects,
                SubjectsByCredits = subjectsByCredits
            };

            return result;
        }
    }
}

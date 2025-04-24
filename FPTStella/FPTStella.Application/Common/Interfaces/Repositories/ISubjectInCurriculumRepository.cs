using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface ISubjectInCurriculumRepository : IRepository<SubjectInCurriculum>
    {
        /// <summary>
        /// Lấy danh sách SubjectInCurriculum theo Curriculum Id.
        /// </summary>
        /// <param name="curriculumId">Id của Curriculum.</param>
        /// <returns>Danh sách SubjectInCurriculum.</returns>
        Task<List<SubjectInCurriculum>> GetByCurriculumIdAsync(Guid curriculumId);
        /// <summary>
        /// Lấy danh sách SubjectInCurriculum theo Subject Id.
        /// </summary>
        /// <param name="subjectId">Id của Subject.</param>
        /// <returns>Danh sách SubjectInCurriculum.</returns>
        Task<List<SubjectInCurriculum>> GetBySubjectIdAsync(Guid subjectId);
        /// <summary>
        /// Kiểm tra xem một mối quan hệ giữa Subject và Curriculum đã tồn tại hay chưa.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="curriculumId"></param>
        /// <returns></returns>
        Task<bool> IsMappingExistedAsync(Guid subjectId, Guid curriculumId);
        /// <summary>
        /// Xoá mối quan hệ giữa Subject và Curriculum theo Subject Id.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        Task DeleteMappingsBySubjectIdAsync(Guid subjectId);
        /// <summary>
        /// Xoá mối quan hệ giữa Subject và Curriculum theo Curriculum Id.
        /// </summary>
        /// <param name="curriculumId"></param>
        /// <returns></returns>
        Task DeleteMappingsByCurriculumIdAsync(Guid curriculumId);
        /// <summary>
        /// Lấy danh sách Curriculum Ids liên kết với một Subject.
        /// </summary>
        /// <param name="subjectId"></param>
        /// <returns></returns>
        Task<List<Guid>> GetCurriculumIdsBySubjectIdAsync(Guid subjectId);
        /// <summary>
        /// Lấy danh sách Subject Ids liên kết với một Curriculum.
        /// </summary>
        /// <param name="curriculumId"></param>
        /// <returns></returns>
        Task<List<Guid>> GetSubjectIdsByCurriculumIdAsync(Guid curriculumId);
    }
}

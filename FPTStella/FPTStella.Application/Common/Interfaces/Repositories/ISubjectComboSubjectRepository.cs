using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface ISubjectComboSubjectRepository : IRepository<SubjectComboSubjects>
    {
        /// <summary>
        /// Gets all subject mappings for a specific subject combo
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        /// <returns>List of subject combo subject mappings</returns>
        Task<List<SubjectComboSubjects>> GetBySubjectComboIdAsync(Guid subjectComboId);

        /// <summary>
        /// Gets all combo mappings for a specific subject
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of subject combo subject mappings</returns>
        Task<List<SubjectComboSubjects>> GetBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Checks if a mapping between a subject combo and a subject already exists
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>True if the mapping exists, otherwise false</returns>
        Task<bool> IsMappingExistedAsync(Guid subjectComboId, Guid subjectId);

        /// <summary>
        /// Removes all subject mappings for a specific subject combo
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        Task DeleteMappingsBySubjectComboIdAsync(Guid subjectComboId);

        /// <summary>
        /// Removes all combo mappings for a specific subject
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        Task DeleteMappingsBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Gets all subject IDs associated with a specific subject combo
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        /// <returns>List of subject IDs</returns>
        Task<List<Guid>> GetSubjectIdsBySubjectComboIdAsync(Guid subjectComboId);

        /// <summary>
        /// Gets all subject combo IDs that contain a specific subject
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of subject combo IDs</returns>
        Task<List<Guid>> GetSubjectComboIdsBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Searches for mappings with pagination and filtering
        /// </summary>
        /// <param name="subjectComboId">Optional subject combo ID filter</param>
        /// <param name="subjectId">Optional subject ID filter</param>
        /// <param name="paginationParams">Pagination parameters</param>
        /// <returns>Paginated results of subject combo subject mappings</returns>
        Task<PagedResult<SubjectComboSubjects>> SearchMappingsAsync(
              Guid? subjectComboId = null,
              Guid? subjectId = null,
              PaginationParams? paginationParams = null);
    }
}

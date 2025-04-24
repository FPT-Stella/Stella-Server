using FPTStella.Contracts.DTOs.SubjectInCurriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Interface for SubjectInCurriculum service that manages the relationships between subjects and curricula.
    /// </summary>
    public interface ISubjectInCurriculumService
    {
        /// <summary>
        /// Creates a new mapping between a subject and a curriculum.
        /// </summary>
        /// <param name="createDto">Data for creating the mapping</param>
        /// <returns>The created mapping as a DTO</returns>
        Task<SubjectInCurriculumDto> CreateMappingAsync(CreateSubjectInCurriculumDto createDto);

        /// <summary>
        /// Updates an existing subject-curriculum mapping.
        /// </summary>
        /// <param name="id">ID of the mapping to update</param>
        /// <param name="updateDto">Updated mapping data</param>
        Task UpdateMappingAsync(Guid id, UpdateSubjectInCurriculumDto updateDto);

        /// <summary>
        /// Gets a specific subject-curriculum mapping by its ID.
        /// </summary>
        /// <param name="id">The ID of the mapping</param>
        /// <returns>The mapping as a DTO</returns>
        Task<SubjectInCurriculumDto> GetMappingByIdAsync(Guid id);

        /// <summary>
        /// Gets all active subject-curriculum mappings.
        /// </summary>
        /// <returns>List of all active mappings</returns>
        Task<List<SubjectInCurriculumDto>> GetAllMappingsAsync();

        /// <summary>
        /// Gets all mappings for a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>List of mappings for the curriculum</returns>
        Task<List<SubjectInCurriculumDto>> GetMappingsByCurriculumIdAsync(Guid curriculumId);

        /// <summary>
        /// Gets all mappings for a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of mappings for the subject</returns>
        Task<List<SubjectInCurriculumDto>> GetMappingsBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Gets subject IDs that are mapped to a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>List of subject IDs</returns>
        Task<List<Guid>> GetSubjectIdsByCurriculumIdAsync(Guid curriculumId);

        /// <summary>
        /// Gets curriculum IDs that are mapped to a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of curriculum IDs</returns>
        Task<List<Guid>> GetCurriculumIdsBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Checks if a mapping between a subject and curriculum exists.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>True if mapping exists, otherwise false</returns>
        Task<bool> IsMappingExistedAsync(Guid subjectId, Guid curriculumId);

        /// <summary>
        /// Deletes a mapping by its ID.
        /// </summary>
        /// <param name="id">The ID of the mapping to delete</param>
        Task DeleteMappingAsync(Guid id);

        /// <summary>
        /// Deletes all mappings for a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        Task DeleteMappingsBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Deletes all mappings for a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        Task DeleteMappingsByCurriculumIdAsync(Guid curriculumId);
    }
}

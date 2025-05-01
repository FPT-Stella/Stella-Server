using FPTStella.Contracts.DTOs.SubjectComboSubjects;
using FPTStella.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface ISubjectComboSubjectService
    {
        /// <summary>
        /// Creates a new mapping between a subject combo and a subject
        /// </summary>
        /// <param name="createDto">The data for the new mapping</param>
        /// <returns>The created mapping as a DTO</returns>
        Task<SubjectComboSubjectDto> CreateMappingAsync(CreateSubjectComboSubjectDto createDto);

        /// <summary>
        /// Gets a mapping by its ID
        /// </summary>
        /// <param name="id">The ID of the mapping</param>
        /// <returns>The mapping as a DTO if found</returns>
        Task<SubjectComboSubjectDto> GetMappingByIdAsync(Guid id);

        /// <summary>
        /// Gets all mappings for a specific subject combo
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        /// <returns>List of mappings as DTOs</returns>
        Task<List<SubjectComboSubjectDto>> GetMappingsBySubjectComboIdAsync(Guid subjectComboId);

        /// <summary>
        /// Gets all mappings for a specific subject
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of mappings as DTOs</returns>
        Task<List<SubjectComboSubjectDto>> GetMappingsBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Gets all mappings
        /// </summary>
        /// <returns>List of all mappings as DTOs</returns>
        Task<List<SubjectComboSubjectDto>> GetAllMappingsAsync();

        /// <summary>
        /// Deletes a mapping by its ID
        /// </summary>
        /// <param name="id">The ID of the mapping to delete</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> DeleteMappingAsync(Guid id);

        /// <summary>
        /// Deletes all mappings for a specific subject combo
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> DeleteMappingsBySubjectComboIdAsync(Guid subjectComboId);

        /// <summary>
        /// Deletes all mappings for a specific subject
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> DeleteMappingsBySubjectIdAsync(Guid subjectId);

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
        /// Checks if a mapping between a subject combo and a subject already exists
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>True if the mapping exists, otherwise false</returns>
        Task<bool> IsMappingExistedAsync(Guid subjectComboId, Guid subjectId);

        /// <summary>
        /// Searches for subject combo subject mappings with pagination
        /// </summary>
        /// <param name="subjectComboId">Optional subject combo ID filter</param>
        /// <param name="subjectId">Optional subject ID filter</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged result of mappings as DTOs</returns>
        Task<PagedResult<SubjectComboSubjectDto>> SearchMappingsAsync(
            Guid? subjectComboId = null,
            Guid? subjectId = null,
            int pageNumber = 1,
            int pageSize = 10);
        // Add to ISubjectComboSubjectService.cs
        /// <summary>
        /// Creates multiple mappings between subject combos and subjects in a batch operation
        /// </summary>
        /// <param name="batchDto">The batch of mappings to create</param>
        /// <returns>The successfully created mappings</returns>
        Task<CreateSubjectComboSubjectBatchDto> CreateMappingBatchAsync(CreateSubjectComboSubjectBatchDto batchDto);

        /// <summary>
        /// Updates multiple mappings between subject combos and subjects in a batch operation
        /// </summary>
        /// <param name="batchDto">The batch of mappings to update</param>
        /// <returns>Result containing information about successful and failed updates</returns>
        Task<UpdateSubjectComboSubjectResultDto> UpdateMappingsBatchAsync(UpdateSubjectComboSubjectBatchDto batchDto);

        /// <summary>
        /// Updates all subject mappings for a specific subject combo
        /// </summary>
        /// <param name="dto">The patch DTO containing subject combo ID and new subject IDs</param>
        Task UpdateSubjectComboMappingAsync(PatchSubjectComboMappingDto dto);
        Task UpdateSubjectMappingAsync(PatchSubjectMappingDto dto);
    }
}

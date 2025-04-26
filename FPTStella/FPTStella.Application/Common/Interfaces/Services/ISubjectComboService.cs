using FPTStella.Contracts.DTOs.SubjectCombos;
using FPTStella.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    /// <summary>
    /// Service interface for Subject Combo operations
    /// </summary>
    public interface ISubjectComboService
    {
        /// <summary>
        /// Creates a new subject combo
        /// </summary>
        /// <param name="createDto">The data for the new subject combo</param>
        /// <returns>The created subject combo as a DTO</returns>
        Task<SubjectComboDto> CreateComboAsync(CreateSubjectComboDto createDto);

        /// <summary>
        /// Gets a subject combo by its ID
        /// </summary>
        /// <param name="id">The ID of the subject combo</param>
        /// <returns>The subject combo as a DTO</returns>
        Task<SubjectComboDto> GetComboByIdAsync(Guid id);

        /// <summary>
        /// Gets a subject combo by its name
        /// </summary>
        /// <param name="comboName">The name of the subject combo</param>
        /// <returns>The subject combo as a DTO</returns>
        Task<SubjectComboDto> GetComboByNameAsync(string comboName);

        /// <summary>
        /// Gets all subject combos for a specific program
        /// </summary>
        /// <param name="programId">The program ID</param>
        /// <returns>A list of subject combos as DTOs</returns>
        Task<List<SubjectComboDto>> GetCombosByProgramIdAsync(Guid programId);

        /// <summary>
        /// Updates a subject combo
        /// </summary>
        /// <param name="id">The ID of the subject combo to update</param>
        /// <param name="updateDto">The update data</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> UpdateComboAsync(Guid id, UpdateSubjectComboDto updateDto);

        /// <summary>
        /// Deletes a subject combo
        /// </summary>
        /// <param name="id">The ID of the subject combo to delete</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> DeleteComboAsync(Guid id);

        /// <summary>
        /// Deletes all subject combos for a specific program
        /// </summary>
        /// <param name="programId">The program ID</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> DeleteCombosByProgramIdAsync(Guid programId);

        /// <summary>
        /// Searches for subject combos with pagination
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <param name="programId">Optional program ID filter</param>
        /// <param name="pageNumber">Page number</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Paged result of subject combos as DTOs</returns>
        Task<PagedResult<SubjectComboDto>> SearchCombosAsync(
            string searchTerm,
            Guid? programId,
            int pageNumber = 1,
            int pageSize = 10);

        /// <summary>
        /// Checks if a combo name already exists within a program
        /// </summary>
        /// <param name="programId">The program ID</param>
        /// <param name="comboName">The combo name to check</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> IsComboNameExistedAsync(Guid programId, string comboName);

    }
}

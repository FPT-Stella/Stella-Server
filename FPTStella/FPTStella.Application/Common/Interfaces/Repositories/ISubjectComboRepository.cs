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
    /// <summary>
    /// Repository interface for Subject Combo operations
    /// </summary>
    public interface ISubjectComboRepository : IRepository<SubjectCombo>
    {
        /// <summary>
        /// Gets Subject Combos by Program ID
        /// </summary>
        /// <param name="programId">The program ID</param>
        /// <returns>List of Subject Combos</returns>
        Task<List<SubjectCombo>> GetByProgramIdAsync(Guid programId);

        /// <summary>
        /// Gets Subject Combo by combo name
        /// </summary>
        /// <param name="comboName">The combo name to search for</param>
        /// <returns>The Subject Combo if found, otherwise null</returns>
        Task<SubjectCombo?> GetByComboNameAsync(string comboName);

        /// <summary>
        /// Checks if a Subject Combo with the specified name exists within a program
        /// </summary>
        /// <param name="programId">The program ID</param>
        /// <param name="comboName">The combo name</param>
        /// <returns>True if exists, otherwise false</returns>
        Task<bool> IsComboNameExistedInProgramAsync(Guid programId, string comboName);

        /// <summary>
        /// Searches Subject Combos with pagination
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <param name="programId">Optional program ID filter</param>
        /// <param name="paginationParams">Pagination parameters</param>
        /// <returns>Paged result of Subject Combos</returns>
        Task<PagedResult<SubjectCombo>> SearchComboAsync(
            string searchTerm,
            Guid? programId,
            PaginationParams paginationParams);

        /// <summary>
        /// Deletes all Subject Combos for a specific program
        /// </summary>
        /// <param name="programId">The program ID</param>
        Task DeleteByProgramIdAsync(Guid programId);

    }
}

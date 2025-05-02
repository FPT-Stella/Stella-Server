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
    public interface IMaterialRepository : IRepository<Materials>
    {
        /// <summary>
        /// Gets materials by subject ID
        /// </summary>
        /// <param name="subjectId">The subject ID to search for</param>
        /// <returns>List of materials related to the subject</returns>
        Task<List<Materials>> GetBySubjectIdAsync(Guid subjectId);
        /// <summary>
        /// Gets a material by its name
        /// </summary>
        /// <param name="materialName">The material name to search for</param>
        /// <returns>The material if found, otherwise null</returns>
        Task<Materials?> GetByMaterialNameAsync(string materialName);
        /// <summary>
        /// Gets materials by material type
        /// </summary>
        /// <param name="materialType">The material type to search for</param>
        /// <returns>List of materials of the specified type</returns>
        Task<List<Materials>> GetByMaterialTypeAsync(string materialType);
        /// <summary>
        /// Searches for materials with advanced filtering options and pagination
        /// </summary>
        /// <param name="searchTerm">Optional search term for text-based fields</param>
        /// <param name="subjectId">Optional subject ID filter</param>
        /// <param name="materialType">Optional material type filter</param>
        /// <param name="paginationParams">Pagination parameters</param>
        /// <returns>Paginated results of materials</returns>
        Task<PagedResult<Materials>> SearchMaterialsAsync(
            string? searchTerm = null,
            Guid? subjectId = null,
            string? materialType = null,
            PaginationParams? paginationParams = null);
    }
}

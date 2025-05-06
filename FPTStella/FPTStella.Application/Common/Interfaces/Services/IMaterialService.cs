using FPTStella.Contracts.DTOs.Materials;
using FPTStella.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IMaterialService
    {
        /// <summary>
        /// Creates a new material
        /// </summary>
        /// <param name="createMaterialDto">The data for the new material</param>
        /// <returns>The created material as a DTO</returns>
        Task<MaterialDto> CreateMaterialAsync(CreateMaterialDto createMaterialDto);
        /// <summary>
        /// Gets a material by its ID
        /// </summary>
        /// <param name="id">The ID of the material</param>
        /// <returns>The material as a DTO if found</returns>
        Task<MaterialDto> GetMaterialByIdAsync(string id);
        /// <summary>
        /// Gets a material by its name
        /// </summary>
        /// <param name="materialName">The name of the material</param>
        /// <returns>The material as a DTO if found, otherwise null</returns>
        Task<MaterialDto?> GetMaterialByNameAsync(string materialName);
        /// <summary>
        /// Gets all materials for a specific subject
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of materials as DTOs</returns>
        Task<List<MaterialDto>> GetMaterialsBySubjectIdAsync(Guid subjectId);
        /// <summary>
        /// Gets all materials with a specific type
        /// </summary>
        /// <param name="materialType">The material type</param>
        /// <returns>List of materials as DTOs</returns>
        Task<List<MaterialDto>> GetMaterialsByTypeAsync(string materialType);
        /// <summary>
        /// Gets all materials
        /// </summary>
        /// <returns>List of all materials as DTOs</returns>
        Task<List<MaterialWithSubjectCodeDto>> GetAllMaterialsAsync();
        /// <summary>
        /// Updates a material
        /// </summary>
        /// <param name="id">The ID of the material to update</param>
        /// <param name="updateMaterialDto">The update data</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> UpdateMaterialAsync(string id, UpdateMaterialDto updateMaterialDto);
        /// <summary>
        /// Deletes a material by its ID
        /// </summary>
        /// <param name="id">The ID of the material to delete</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> DeleteMaterialAsync(string id);
        /// <summary>
        /// Deletes all materials for a specific subject
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> DeleteMaterialsBySubjectIdAsync(Guid subjectId);
        /// <summary>
        /// Searches for materials with pagination
        /// </summary>
        /// <param name="searchTerm">Optional search term for material name or description</param>
        /// <param name="subjectId">Optional subject ID filter</param>
        /// <param name="materialType">Optional material type filter</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged result of materials as DTOs</returns>
        Task<PagedResult<MaterialDto>> SearchMaterialsAsync(
            string? searchTerm = null,
            Guid? subjectId = null,
            string? materialType = null,
            int pageNumber = 1,
            int pageSize = 10);
    }
}

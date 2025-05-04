using FPTStella.Contracts.DTOs.CLOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface ICLOService
    {
        /// <summary>
        /// Creates a new CLO.
        /// </summary>
        /// <param name="createCLODto">The DTO containing CLO creation data</param>
        /// <returns>The newly created CLO as a DTO</returns>
        Task<CLOWithDetailsDto> CreateCLOAsync(CreateCLODto createCLODto);

        /// <summary>
        /// Gets all active CLOs.
        /// </summary>
        /// <returns>A list of all active CLOs as DTOs</returns>
        Task<List<CLOWithDetailsDto>> GetAllCLOsAsync();

        /// <summary>
        /// Gets a CLO by its ID.
        /// </summary>
        /// <param name="id">The CLO ID</param>
        /// <returns>The CLO as a DTO</returns>
        Task<CLOWithDetailsDto> GetCLOByIdAsync(Guid id);

        /// <summary>
        /// Gets CLOs by a Subject ID.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <returns>A list of CLOs as DTOs</returns>
        Task<List<CLOWithDetailsDto>> GetCLOsBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Gets CLOs by a list of Subject IDs.
        /// </summary>
        /// <param name="subjectIds">The list of Subject IDs</param>
        /// <returns>A list of CLOs as DTOs</returns>
        Task<List<CLOWithDetailsDto>> GetCLOsBySubjectIdsAsync(List<Guid> subjectIds);

        /// <summary>
        /// Checks if a CLO with the given name exists for a specific subject.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <param name="cloName">The CLO name</param>
        /// <returns>True if the CLO exists, false otherwise</returns>
        Task<bool> IsCloNameExistedAsync(Guid subjectId, string cloName);

        /// <summary>
        /// Checks if a CLO with the given details exists for a specific subject.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <param name="cloDetails">The CLO details</param>
        /// <returns>True if the CLO exists, false otherwise</returns>
        Task<bool> IsCloDetailsExistedAsync(Guid subjectId, string cloDetails);

        /// <summary>
        /// Updates an existing CLO.
        /// </summary>
        /// <param name="id">The CLO ID</param>
        /// <param name="updateCLODto">The DTO containing update data</param>
        Task UpdateCLOAsync(Guid id, UpdateCLODto updateCLODto);

        /// <summary>
        /// Soft deletes a CLO by setting its DeleteFlag.
        /// </summary>
        /// <param name="id">The CLO ID</param>
        Task DeleteCLOAsync(Guid id);

        /// <summary>
        /// Deletes all CLOs by Subject ID.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        Task DeleteCLOsBySubjectIdAsync(Guid subjectId);
    }
}
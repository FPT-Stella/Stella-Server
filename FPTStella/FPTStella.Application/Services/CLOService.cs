using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.CLOs;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class CLOService : ICLOService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICLORepository _cloRepository;

        /// <summary>
        /// Initializes a new instance of the CLOService class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        /// <param name="cloRepository">The CLO repository</param>
        public CLOService(IUnitOfWork unitOfWork, ICLORepository cloRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _cloRepository = cloRepository ?? throw new ArgumentNullException(nameof(cloRepository));
        }

        /// <summary>
        /// Maps a CLO entity to its DTO representation.
        /// </summary>
        /// <param name="clo">The CLO entity</param>
        /// <returns>A mapped CLOWithDetailsDto object</returns>
        private static CLOWithDetailsDto MapToCLOWithDetailsDto(CLOs clo) => new()
        {
            Id = clo.Id,
            SubjectId = clo.SubjectId,
            CloName = clo.CloName,
            CloDetails = clo.CloDetails,
            LoDetails = clo.LoDetails
        };

        /// <summary>
        /// Creates a new CLO.
        /// </summary>
        /// <param name="createCLODto">The DTO containing CLO creation data</param>
        /// <returns>The newly created CLO as a DTO</returns>
        /// <exception cref="InvalidOperationException">Thrown when a CLO with the same name or details already exists</exception>
        public async Task<CLOWithDetailsDto> CreateCLOAsync(CreateCLODto createCLODto)
        {
            if (await _cloRepository.IsCloNameExistedAsync(createCLODto.SubjectId, createCLODto.CloName))
            {
                throw new InvalidOperationException("CLO with the same name already exists for the subject.");
            }

            if (await _cloRepository.IsCloDetailsExistedAsync(createCLODto.SubjectId, createCLODto.CloDetails))
            {
                throw new InvalidOperationException("CLO with the same details already exists for the subject.");
            }

            var clo = new CLOs
            {
                SubjectId = createCLODto.SubjectId,
                CloName = createCLODto.CloName,
                CloDetails = createCLODto.CloDetails,
                LoDetails = createCLODto.LoDetails,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _cloRepository.InsertAsync(clo);
            await _unitOfWork.SaveAsync();

            return MapToCLOWithDetailsDto(clo);
        }

        /// <summary>
        /// Gets all active CLOs.
        /// </summary>
        /// <returns>A list of all active CLOs as DTOs</returns>
        public async Task<List<CLOWithDetailsDto>> GetAllCLOsAsync()
        {
            var clos = await _cloRepository.FilterByAsync(c => !c.DelFlg);
            return clos.Select(MapToCLOWithDetailsDto).ToList();
        }

        /// <summary>
        /// Gets a CLO by its ID.
        /// </summary>
        /// <param name="id">The CLO ID</param>
        /// <returns>The CLO as a DTO</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the CLO is not found</exception>
        public async Task<CLOWithDetailsDto> GetCLOByIdAsync(Guid id)
        {
            var clo = await _cloRepository.GetByIdAsync(id.ToString());

            if (clo == null || clo.DelFlg)
            {
                throw new KeyNotFoundException("CLO not found.");
            }

            return MapToCLOWithDetailsDto(clo);
        }

        /// <summary>
        /// Gets CLOs by a Subject ID.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <returns>A list of CLOs as DTOs</returns>
        public async Task<List<CLOWithDetailsDto>> GetCLOsBySubjectIdAsync(Guid subjectId)
        {
            var clos = await _cloRepository.GetBySubjectIdAsync(subjectId);
            return clos.Select(MapToCLOWithDetailsDto).ToList();
        }

        /// <summary>
        /// Gets CLOs by a list of Subject IDs.
        /// </summary>
        /// <param name="subjectIds">The list of Subject IDs</param>
        /// <returns>A list of CLOs as DTOs</returns>
        public async Task<List<CLOWithDetailsDto>> GetCLOsBySubjectIdsAsync(List<Guid> subjectIds)
        {
            var clos = await _cloRepository.GetBySubjectIdsAsync(subjectIds);
            return clos.Select(MapToCLOWithDetailsDto).ToList();
        }

        /// <summary>
        /// Checks if a CLO with the given name exists for a specific subject.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <param name="cloName">The CLO name</param>
        /// <returns>True if the CLO exists, false otherwise</returns>
        public async Task<bool> IsCloNameExistedAsync(Guid subjectId, string cloName)
        {
            return await _cloRepository.IsCloNameExistedAsync(subjectId, cloName);
        }

        /// <summary>
        /// Checks if a CLO with the given details exists for a specific subject.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <param name="cloDetails">The CLO details</param>
        /// <returns>True if the CLO exists, false otherwise</returns>
        public async Task<bool> IsCloDetailsExistedAsync(Guid subjectId, string cloDetails)
        {
            return await _cloRepository.IsCloDetailsExistedAsync(subjectId, cloDetails);
        }

        /// <summary>
        /// Updates an existing CLO.
        /// </summary>
        /// <param name="id">The CLO ID</param>
        /// <param name="updateCLODto">The DTO containing update data</param>
        /// <exception cref="KeyNotFoundException">Thrown when the CLO is not found</exception>
        /// <exception cref="InvalidOperationException">Thrown when a CLO with the same name or details already exists</exception>
        public async Task UpdateCLOAsync(Guid id, UpdateCLODto updateCLODto)
        {
            var clo = await _cloRepository.GetByIdAsync(id.ToString());

            if (clo == null || clo.DelFlg)
            {
                throw new KeyNotFoundException("CLO not found.");
            }

            if (!string.IsNullOrWhiteSpace(updateCLODto.CloName) &&
                updateCLODto.CloName != clo.CloName &&
                await _cloRepository.IsCloNameExistedAsync(clo.SubjectId, updateCLODto.CloName))
            {
                throw new InvalidOperationException("CLO with the same name already exists for the subject.");
            }

            if (!string.IsNullOrWhiteSpace(updateCLODto.CloDetails) &&
                updateCLODto.CloDetails != clo.CloDetails &&
                await _cloRepository.IsCloDetailsExistedAsync(clo.SubjectId, updateCLODto.CloDetails))
            {
                throw new InvalidOperationException("CLO with the same details already exists for the subject.");
            }

            clo.CloName = updateCLODto.CloName ?? clo.CloName;
            clo.CloDetails = updateCLODto.CloDetails ?? clo.CloDetails;
            clo.LoDetails = updateCLODto.LoDetails ?? clo.LoDetails;
            clo.UpdDate = DateTime.UtcNow;

            await _cloRepository.ReplaceAsync(id.ToString(), clo);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Soft deletes a CLO by setting its DeleteFlag.
        /// </summary>
        /// <param name="id">The CLO ID</param>
        /// <exception cref="KeyNotFoundException">Thrown when the CLO is not found</exception>
        public async Task DeleteCLOAsync(Guid id)
        {
            var clo = await _cloRepository.GetByIdAsync(id.ToString());

            if (clo == null || clo.DelFlg)
            {
                throw new KeyNotFoundException("CLO not found.");
            }

            clo.DelFlg = true;
            clo.UpdDate = DateTime.UtcNow;

            await _cloRepository.ReplaceAsync(id.ToString(), clo);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Deletes all CLOs by Subject ID.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        public async Task DeleteCLOsBySubjectIdAsync(Guid subjectId)
        {
            await _cloRepository.DeleteBySubjectIdAsync(subjectId);
            await _unitOfWork.SaveAsync();
        }
    }
}
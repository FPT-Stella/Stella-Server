using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.PLOs;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class PLOService : IPLOService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPLORepository _ploRepository;

        /// <summary>
        /// Initializes a new instance of the PLOService class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        /// <param name="ploRepository">The PLO repository</param>
        public PLOService(IUnitOfWork unitOfWork, IPLORepository ploRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _ploRepository = ploRepository ?? throw new ArgumentNullException(nameof(ploRepository));
        }

        /// <summary>
        /// Maps a PLO entity to its DTO representation.
        /// </summary>
        /// <param name="plo">The PLO entity</param>
        /// <returns>A mapped PLOsDto object</returns>
        private static PLOsDto MapToPLOsDto(PLOs plo) => new()
        {
            Id = plo.Id,
            CurriculumId = plo.CurriculumId,
            PloName = plo.PloName,
            Description = plo.Description
        };

        /// <summary>
        /// Creates a new PLO.
        /// </summary>
        /// <param name="createPLOsDto">The DTO containing PLO creation data</param>
        /// <returns>The newly created PLO as a DTO</returns>
        /// <exception cref="InvalidOperationException">Thrown when a PLO with the same name already exists</exception>
        public async Task<PLOsDto> CreatePLOAsync(CreatePLOsDto createPLOsDto)
        {
            if (await _ploRepository.IsPloNameExistedAsync(createPLOsDto.CurriculumId, createPLOsDto.PloName))
            {
                throw new InvalidOperationException("PLO with the same name already exists in the curriculum.");
            }

            var plo = new PLOs
            {
                CurriculumId = createPLOsDto.CurriculumId,
                PloName = createPLOsDto.PloName,
                Description = createPLOsDto.Description,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _ploRepository.InsertAsync(plo);
            await _unitOfWork.SaveAsync();

            return MapToPLOsDto(plo);
        }

        /// <summary>
        /// Gets all active PLOs.
        /// </summary>
        /// <returns>A list of all active PLOs as DTOs</returns>
        public async Task<List<PLOsDto>> GetAllPLOsAsync()
        {
            var plos = await _ploRepository.FilterByAsync(p => !p.DelFlg);
            return plos.Select(MapToPLOsDto).ToList();
        }

        /// <summary>
        /// Gets a PLO by its ID.
        /// </summary>
        /// <param name="id">The PLO ID</param>
        /// <returns>The PLO as a DTO</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the PLO is not found</exception>
        public async Task<PLOsDto> GetPLOByIdAsync(Guid id)
        {
            var plo = await _ploRepository.GetByIdAsync(id.ToString());

            if (plo == null || plo.DelFlg)
            {
                throw new KeyNotFoundException("PLO not found.");
            }

            return MapToPLOsDto(plo);
        }

        /// <summary>
        /// Gets PLOs by a curriculum ID.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>A list of PLOs as DTOs</returns>
        public async Task<List<PLOsDto>> GetPLOsByCurriculumIdAsync(Guid curriculumId)
        {
            var plos = await _ploRepository.GetByCurriculumIdAsync(curriculumId);
            return plos.Select(MapToPLOsDto).ToList();
        }

        /// <summary>
        /// Gets PLOs by a list of curriculum IDs.
        /// </summary>
        /// <param name="curriculumIds">The list of curriculum IDs</param>
        /// <returns>A list of PLOs as DTOs</returns>
        public async Task<List<PLOsDto>> GetPLOsByCurriculumIdsAsync(List<Guid> curriculumIds)
        {
            var plos = await _ploRepository.GetByCurriculumIdsAsync(curriculumIds);
            return plos.Select(MapToPLOsDto).ToList();
        }

        /// <summary>
        /// Checks if a PLO with the given name exists in a curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <param name="ploName">The PLO name</param>
        /// <returns>True if the PLO name exists, false otherwise</returns>
        public async Task<bool> IsPloNameExistedAsync(Guid curriculumId, string ploName)
        {
            return await _ploRepository.IsPloNameExistedAsync(curriculumId, ploName);
        }

        /// <summary>
        /// Updates an existing PLO.
        /// </summary>
        /// <param name="id">The PLO ID</param>
        /// <param name="updatePLOsDto">The DTO containing update data</param>
        /// <exception cref="KeyNotFoundException">Thrown when the PLO is not found</exception>
        /// <exception cref="InvalidOperationException">Thrown when a PLO with the same name already exists</exception>
        public async Task UpdatePLOAsync(Guid id, UpdatePLOsDto updatePLOsDto)
        {
            var plo = await _ploRepository.GetByIdAsync(id.ToString());

            if (plo == null || plo.DelFlg)
            {
                throw new KeyNotFoundException("PLO not found.");
            }

            if (!string.IsNullOrWhiteSpace(updatePLOsDto.PloName) &&
                updatePLOsDto.PloName != plo.PloName &&
                await _ploRepository.IsPloNameExistedAsync(plo.CurriculumId, updatePLOsDto.PloName))
            {
                throw new InvalidOperationException("PLO with the same name already exists in the curriculum.");
            }

            plo.PloName = updatePLOsDto.PloName ?? plo.PloName;
            plo.Description = updatePLOsDto.Description ?? plo.Description;
            plo.UpdDate = DateTime.UtcNow;

            await _ploRepository.ReplaceAsync(id.ToString(), plo);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Soft deletes a PLO by setting its DeleteFlag.
        /// </summary>
        /// <param name="id">The PLO ID</param>
        /// <exception cref="KeyNotFoundException">Thrown when the PLO is not found</exception>
        public async Task DeletePLOAsync(Guid id)
        {
            var plo = await _ploRepository.GetByIdAsync(id.ToString());

            if (plo == null || plo.DelFlg)
            {
                throw new KeyNotFoundException("PLO not found.");
            }

            plo.DelFlg = true;
            plo.UpdDate = DateTime.UtcNow;

            await _ploRepository.ReplaceAsync(id.ToString(), plo);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Deletes all PLOs by curriculum ID.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        public async Task DeletePLOsByCurriculumIdAsync(Guid curriculumId)
        {
            await _ploRepository.DeleteByCurriculumIdAsync(curriculumId);
            await _unitOfWork.SaveAsync();
        }
    }
}

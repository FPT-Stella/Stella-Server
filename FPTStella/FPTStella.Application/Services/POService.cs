using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.POs;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class POService : IPOService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPORepository _poRepository;

        /// <summary>
        /// Initializes a new instance of the POService class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        /// <param name="poRepository">The PO repository</param>
        public POService(IUnitOfWork unitOfWork, IPORepository poRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _poRepository = poRepository ?? throw new ArgumentNullException(nameof(poRepository));
        }

        /// <summary>
        /// Maps a PO entity to its DTO representation.
        /// </summary>
        /// <param name="po">The PO entity</param>
        /// <returns>A mapped POsDto object</returns>
        private static POsDto MapToPOsDto(POs po) => new()
        {
            Id = po.Id,
            ProgramId = po.ProgramId,
            PoName = po.PoName,
            Description = po.Description
        };

        /// <summary>
        /// Creates a new PO.
        /// </summary>
        /// <param name="createPOsDto">The DTO containing PO creation data</param>
        /// <returns>The newly created PO as a DTO</returns>
        /// <exception cref="InvalidOperationException">Thrown when a PO with the same name already exists</exception>
        public async Task<POsDto> CreatePOAsync(CreatePOsDto createPOsDto)
        {
            if (await _poRepository.IsPoNameExistedAsync(createPOsDto.ProgramId, createPOsDto.PoName))
            {
                throw new InvalidOperationException("PO with the same name already exists in the program.");
            }

            var po = new POs
            {
                ProgramId = createPOsDto.ProgramId,
                PoName = createPOsDto.PoName,
                Description = createPOsDto.Description,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _poRepository.InsertAsync(po);
            await _unitOfWork.SaveAsync();

            return MapToPOsDto(po);
        }

        /// <summary>
        /// Gets all active POs.
        /// </summary>
        /// <returns>A list of all active POs as DTOs</returns>
        public async Task<List<POsDto>> GetAllPOsAsync()
        {
            var pos = await _poRepository.FilterByAsync(p => !p.DelFlg);
            return pos.Select(MapToPOsDto).ToList();
        }

        /// <summary>
        /// Gets a PO by its ID.
        /// </summary>
        /// <param name="id">The PO ID</param>
        /// <returns>The PO as a DTO</returns>
        /// <exception cref="KeyNotFoundException">Thrown when the PO is not found</exception>
        public async Task<POsDto> GetPOByIdAsync(Guid id)
        {
            var po = await _poRepository.GetByIdAsync(id.ToString());

            if (po == null || po.DelFlg)
            {
                throw new KeyNotFoundException("PO not found.");
            }

            return MapToPOsDto(po);
        }

        /// <summary>
        /// Gets POs by a program ID.
        /// </summary>
        /// <param name="programId">The program ID</param>
        /// <returns>A list of POs as DTOs</returns>
        public async Task<List<POsDto>> GetPOsByProgramIdAsync(Guid programId)
        {
            var pos = await _poRepository.GetByProgramIdAsync(programId);
            return pos.Select(MapToPOsDto).ToList();
        }

        /// <summary>
        /// Gets POs by a list of program IDs.
        /// </summary>
        /// <param name="programIds">The list of program IDs</param>
        /// <returns>A list of POs as DTOs</returns>
        public async Task<List<POsDto>> GetPOsByProgramIdsAsync(List<Guid> programIds)
        {
            var pos = await _poRepository.GetByProgramIdsAsync(programIds);
            return pos.Select(MapToPOsDto).ToList();
        }

        /// <summary>
        /// Checks if a PO with the given name exists in a program.
        /// </summary>
        /// <param name="programId">The program ID</param>
        /// <param name="poName">The PO name</param>
        /// <returns>True if the PO name exists, false otherwise</returns>
        public async Task<bool> IsPoNameExistedAsync(Guid programId, string poName)
        {
            return await _poRepository.IsPoNameExistedAsync(programId, poName);
        }

        /// <summary>
        /// Updates an existing PO.
        /// </summary>
        /// <param name="id">The PO ID</param>
        /// <param name="updatePOsDto">The DTO containing update data</param>
        /// <exception cref="KeyNotFoundException">Thrown when the PO is not found</exception>
        /// <exception cref="InvalidOperationException">Thrown when a PO with the same name already exists</exception>
        public async Task UpdatePOAsync(Guid id, UpdatePOsDto updatePOsDto)
        {
            var po = await _poRepository.GetByIdAsync(id.ToString());

            if (po == null || po.DelFlg)
            {
                throw new KeyNotFoundException("PO not found.");
            }

            if (!string.IsNullOrWhiteSpace(updatePOsDto.PoName) &&
                updatePOsDto.PoName != po.PoName &&
                await _poRepository.IsPoNameExistedAsync(po.ProgramId, updatePOsDto.PoName))
            {
                throw new InvalidOperationException("PO with the same name already exists in the program.");
            }

            po.PoName = updatePOsDto.PoName ?? po.PoName;
            po.Description = updatePOsDto.Description ?? po.Description;
            po.UpdDate = DateTime.UtcNow;

            await _poRepository.ReplaceAsync(id.ToString(), po);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Soft deletes a PO by setting its DeleteFlag.
        /// </summary>
        /// <param name="id">The PO ID</param>
        /// <exception cref="KeyNotFoundException">Thrown when the PO is not found</exception>
        public async Task DeletePOAsync(Guid id)
        {
            var po = await _poRepository.GetByIdAsync(id.ToString());

            if (po == null || po.DelFlg)
            {
                throw new KeyNotFoundException("PO not found.");
            }

            po.DelFlg = true;
            po.UpdDate = DateTime.UtcNow;

            await _poRepository.ReplaceAsync(id.ToString(), po);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Deletes all POs by program ID.
        /// </summary>
        /// <param name="programId">The program ID</param>
        public async Task DeletePOsByProgramIdAsync(Guid programId)
        {
            await _poRepository.DeleteByProgramIdAsync(programId);
            await _unitOfWork.SaveAsync();
        }
    }
}

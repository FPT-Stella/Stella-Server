using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.PO_PLO_Mappings;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class PO_PLO_MappingService : IPO_PLO_MappingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPO_PLO_MappingRepository _mappingRepository;

        /// <summary>
        /// Initializes a new instance of the PO_PLO_MappingService class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        /// <param name="mappingRepository">The PO_PLO mapping repository</param>
        public PO_PLO_MappingService(IUnitOfWork unitOfWork, IPO_PLO_MappingRepository mappingRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mappingRepository = mappingRepository ?? throw new ArgumentNullException(nameof(mappingRepository));
        }

        /// <summary>
        /// Creates a new mapping between PO and PLO.
        /// </summary>
        /// <param name="createMappingDto">The DTO containing mapping creation data</param>
        /// <exception cref="InvalidOperationException">Thrown when a mapping already exists</exception>
        public async Task CreateMappingAsync(CreatePO_PLO_MappingDto createMappingDto)
        {
            if (await _mappingRepository.IsMappingExistedAsync(createMappingDto.PoId, createMappingDto.PloId))
            {
                throw new InvalidOperationException("Mapping between PO and PLO already exists.");
            }

            var mapping = new PO_PLO_Mapping
            {
                PoId = createMappingDto.PoId,
                PloId = createMappingDto.PloId,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _mappingRepository.InsertAsync(mapping);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Gets PLO IDs associated with a specific PO.
        /// </summary>
        /// <param name="poId">The PO ID</param>
        /// <returns>List of PLO IDs</returns>
        public async Task<List<Guid>> GetPloIdsByPoIdAsync(Guid poId)
        {
            return await _mappingRepository.GetPloIdsByPoIdAsync(poId);
        }

        /// <summary>
        /// Gets PO IDs associated with a specific PLO.
        /// </summary>
        /// <param name="ploId">The PLO ID</param>
        /// <returns>List of PO IDs</returns>
        public async Task<List<Guid>> GetPoIdsByPloIdAsync(Guid ploId)
        {
            return await _mappingRepository.GetPoIdsByPloIdAsync(ploId);
        }

        /// <summary>
        /// Checks if a mapping between specific PO and PLO exists.
        /// </summary>
        /// <param name="poId">The PO ID</param>
        /// <param name="ploId">The PLO ID</param>
        /// <returns>True if the mapping exists, false otherwise</returns>
        public async Task<bool> IsMappingExistedAsync(Guid poId, Guid ploId)
        {
            return await _mappingRepository.IsMappingExistedAsync(poId, ploId);
        }

        /// <summary>
        /// Deletes all mappings associated with a specific PO.
        /// </summary>
        /// <param name="poId">The PO ID</param>
        public async Task DeleteMappingsByPoIdAsync(Guid poId)
        {
            await _mappingRepository.DeleteMappingsByPoIdAsync(poId);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Deletes all mappings associated with a specific PLO.
        /// </summary>
        /// <param name="ploId">The PLO ID</param>
        public async Task DeleteMappingsByPloIdAsync(Guid ploId)
        {
            await _mappingRepository.DeleteMappingsByPloIdAsync(ploId);
            await _unitOfWork.SaveAsync();
        }
    }
}

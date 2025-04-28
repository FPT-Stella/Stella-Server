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
        /// Creates multiple mappings between POs and PLOs in a single operation and returns the created mappings.
        /// </summary>
        /// <param name="createMappingBatchDto">The DTO containing multiple mapping creation data</param>
        /// <returns>The DTO with successfully created mappings</returns>
        public async Task<CreatePO_PLO_MappingBatchDto> CreateMappingBatchAsync(CreatePO_PLO_MappingBatchDto createMappingBatchDto)
        {
            if (createMappingBatchDto.Mappings == null || !createMappingBatchDto.Mappings.Any())
            {
                throw new ArgumentException("No mappings provided for batch creation.");
            }

            var mappingsToInsert = new List<PO_PLO_Mapping>();
            var createdMappings = new List<CreatePO_PLO_MappingDto>();
            var invalidMappings = new List<(CreatePO_PLO_MappingDto Mapping, string Reason)>();
            var now = DateTime.UtcNow;

            var poRepository = _unitOfWork.Repository<POs>();
            var ploRepository = _unitOfWork.Repository<PLOs>();

            // Get all distinct PO IDs and PLO IDs to validate in batches
            var distinctPoIds = createMappingBatchDto.Mappings.Select(m => m.PoId).Distinct().ToList();
            var distinctPloIds = createMappingBatchDto.Mappings.Select(m => m.PloId).Distinct().ToList();

            // Retrieve existing POs and PLOs to validate against
            var existingPOs = new List<POs>();
            var existingPLOs = new List<PLOs>();

            foreach (var poId in distinctPoIds)
            {
                var po = await poRepository.GetByIdAsync(poId.ToString());
                if (po != null)
                {
                    existingPOs.Add(po);
                }
            }

            foreach (var ploId in distinctPloIds)
            {
                var plo = await ploRepository.GetByIdAsync(ploId.ToString());
                if (plo != null)
                {
                    existingPLOs.Add(plo);
                }
            }

            foreach (var mappingDto in createMappingBatchDto.Mappings)
            {
                // Skip if mapping already exists
                if (await _mappingRepository.IsMappingExistedAsync(mappingDto.PoId, mappingDto.PloId))
                {
                    continue;
                }
                // Validate PO and PLO existence
                var poExists = existingPOs.Any(p => p.Id == mappingDto.PoId);
                var ploExists = existingPLOs.Any(p => p.Id == mappingDto.PloId);

                if (!poExists)
                {
                    invalidMappings.Add((mappingDto, $"PO with ID {mappingDto.PoId} does not exist"));
                    continue;
                }

                if (!ploExists)
                {
                    invalidMappings.Add((mappingDto, $"PLO with ID {mappingDto.PloId} does not exist"));
                    continue;
                }
                var mapping = new PO_PLO_Mapping
                {
                    PoId = mappingDto.PoId,
                    PloId = mappingDto.PloId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                };

                mappingsToInsert.Add(mapping);
                createdMappings.Add(new CreatePO_PLO_MappingDto
                {
                    PoId = mappingDto.PoId,
                    PloId = mappingDto.PloId
                });
            }

            if (mappingsToInsert.Any())
            {
                await _mappingRepository.InsertManyAsync(mappingsToInsert);
                await _unitOfWork.SaveAsync();
            }

            // Return only the mappings that were successfully created
            return new CreatePO_PLO_MappingBatchDto
            {
                Mappings = createdMappings
            };
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
        public async Task<List<POWithNameDto>> GetPOsWithNameByPloIdAsync(Guid ploId)
        {
            var poDetailsWithName = await _mappingRepository.GetPOsWithNameByPloIdAsync(ploId);

            return poDetailsWithName.Select(p => new POWithNameDto
            {
                Id = p.Id,
                Name = p.Name
            }).ToList();
        }
        private async Task ValidateIdsExistAsync(Guid poId, Guid ploId)
        {
            var poRepository = _unitOfWork.Repository<POs>();
            var ploRepository = _unitOfWork.Repository<PLOs>();

            var po = await poRepository.GetByIdAsync(poId.ToString());
            if (po == null)
            {
                throw new KeyNotFoundException($"Program Outcome (PO) with ID {poId} does not exist.");
            }

            var plo = await ploRepository.GetByIdAsync(ploId.ToString());
            if (plo == null)
            {
                throw new KeyNotFoundException($"Program Learning Outcome (PLO) with ID {ploId} does not exist.");
            }
        }
    }
}

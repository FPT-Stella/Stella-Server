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
        /// <summary>
        /// Updates multiple mappings between POs and PLOs based on the provided data.
        /// </summary>
        /// <param name="updateMappingBatchDto">The DTO containing multiple mapping update data</param>
        /// <returns>A task representing the asynchronous operation</returns>
        /// <summary>
        /// Updates multiple mappings between POs and PLOs based on the provided data.
        /// </summary>
        /// <param name="updateMappingBatchDto">The DTO containing multiple mapping update data</param>
        /// <returns>A result containing information about updated and failed mappings</returns>
        public async Task<UpdatePO_PLO_MappingResultDto> UpdateMappingsAsync(UpdatePO_PLO_MappingBatchDto updateMappingBatchDto)
        {
            if (updateMappingBatchDto.Mappings == null || !updateMappingBatchDto.Mappings.Any())
            {
                throw new ArgumentException("No mappings provided for update.");
            }

            var now = DateTime.UtcNow;
            var mappingsToUpdate = new List<PO_PLO_Mapping>();
            var updatedMappings = new List<UpdatePO_PLO_MappingDto>();
            var failedMappings = new List<(UpdatePO_PLO_MappingDto Mapping, string Reason)>();

            // Validate PO and PLO existence in batch
            var allPoIds = updateMappingBatchDto.Mappings
                .SelectMany(m => new[] { m.PoId, m.NewPoId })
                .Where(id => id.HasValue && id.Value != Guid.Empty)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var allPloIds = updateMappingBatchDto.Mappings
                .SelectMany(m => new[] { m.PloId, m.NewPloId })
                .Where(id => id.HasValue && id.Value != Guid.Empty)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var poRepository = _unitOfWork.Repository<POs>();
            var ploRepository = _unitOfWork.Repository<PLOs>();

            var existingPOs = new Dictionary<Guid, bool>();
            var existingPLOs = new Dictionary<Guid, bool>();

            foreach (var poId in allPoIds)
            {
                var po = await poRepository.GetByIdAsync(poId.ToString());
                existingPOs[poId] = po != null;
            }

            foreach (var ploId in allPloIds)
            {
                var plo = await ploRepository.GetByIdAsync(ploId.ToString());
                existingPLOs[ploId] = plo != null;
            }

            foreach (var mappingDto in updateMappingBatchDto.Mappings)
            {
                try
                {
                    // Validate the existing mapping
                    var existingMapping = await _mappingRepository.GetMappingAsync(mappingDto.PoId, mappingDto.PloId);
                    if (existingMapping == null)
                    {
                        failedMappings.Add((mappingDto, $"Mapping between PO ID {mappingDto.PoId} and PLO ID {mappingDto.PloId} does not exist."));
                        continue;
                    }

                    // Validate new PO ID if provided
                    if (mappingDto.NewPoId.HasValue && mappingDto.NewPoId.Value != Guid.Empty)
                    {
                        if (!existingPOs.TryGetValue(mappingDto.NewPoId.Value, out var poExists) || !poExists)
                        {
                            failedMappings.Add((mappingDto, $"New PO with ID {mappingDto.NewPoId} does not exist."));
                            continue;
                        }
                    }

                    // Validate new PLO ID if provided
                    if (mappingDto.NewPloId.HasValue && mappingDto.NewPloId.Value != Guid.Empty)
                    {
                        if (!existingPLOs.TryGetValue(mappingDto.NewPloId.Value, out var ploExists) || !ploExists)
                        {
                            failedMappings.Add((mappingDto, $"New PLO with ID {mappingDto.NewPloId} does not exist."));
                            continue;
                        }
                    }

                    // Check if the new mapping already exists
                    if ((mappingDto.NewPoId.HasValue || mappingDto.NewPloId.HasValue) &&
                        await _mappingRepository.IsMappingExistedAsync(
                            mappingDto.NewPoId ?? mappingDto.PoId,
                            mappingDto.NewPloId ?? mappingDto.PloId))
                    {
                        failedMappings.Add((mappingDto, $"A mapping between PO ID {mappingDto.NewPoId ?? mappingDto.PoId} and PLO ID {mappingDto.NewPloId ?? mappingDto.PloId} already exists."));
                        continue;
                    }

                    // Create updated mapping
                    var updatedMapping = new PO_PLO_Mapping
                    {
                        Id = existingMapping.Id,
                        PoId = mappingDto.NewPoId ?? existingMapping.PoId,
                        PloId = mappingDto.NewPloId ?? existingMapping.PloId,
                        InsDate = existingMapping.InsDate,
                        UpdDate = now,
                        DelFlg = existingMapping.DelFlg
                    };

                    mappingsToUpdate.Add(updatedMapping);
                    updatedMappings.Add(mappingDto);
                }
                catch (Exception ex)
                {
                    failedMappings.Add((mappingDto, $"Error updating mapping: {ex.Message}"));
                }
            }

            if (mappingsToUpdate.Any())
            {
                await _mappingRepository.UpdateManyAsync(mappingsToUpdate);
                await _unitOfWork.SaveAsync();
            }

            return new UpdatePO_PLO_MappingResultDto
            {
                UpdatedMappings = updatedMappings,
                FailedMappings = failedMappings.Select(f => new FailedMappingDto
                {
                    Mapping = f.Mapping,
                    Reason = f.Reason
                }).ToList()
            };
        }
        public async Task UpdatePoPloMappingAsync(PatchPloMappingDto dto)
        {
            if (dto == null || dto.PloId == Guid.Empty)
                throw new ArgumentException("Invalid PLO ID");

            // Xóa ánh xạ cũ
            await _mappingRepository.DeleteMappingsByPloIdAsync(dto.PloId);

            // Tạo mới nếu có
            if (dto.PoIds != null && dto.PoIds.Any())
            {
                var now = DateTime.UtcNow;
                var newMappings = dto.PoIds.Select(poId => new PO_PLO_Mapping
                {
                    Id = Guid.NewGuid(),
                    PoId = poId,
                    PloId = dto.PloId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                }).ToList();

                await _mappingRepository.AddManyAsync(newMappings);
            }

            await _unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Updates all PLO mappings for a specific PO by replacing all existing mappings
        /// </summary>
        /// <param name="dto">The DTO containing PO ID and the list of PLO IDs to associate with it</param>
        public async Task UpdatePoMappingAsync(PatchPoMappingDto dto)
        {
            if (dto == null || dto.PoId == Guid.Empty)
                throw new ArgumentException("Invalid PO ID");

            // Delete old mappings
            await _mappingRepository.DeleteMappingsByPoIdAsync(dto.PoId);

            // Create new mappings if there are any PLO IDs provided
            if (dto.PloIds != null && dto.PloIds.Any())
            {
                var now = DateTime.UtcNow;
                var newMappings = dto.PloIds.Select(ploId => new PO_PLO_Mapping
                {
                    Id = Guid.NewGuid(),
                    PoId = dto.PoId,
                    PloId = ploId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                }).ToList();

                await _mappingRepository.AddManyAsync(newMappings);
            }

            await _unitOfWork.SaveAsync();
        }
    }
}

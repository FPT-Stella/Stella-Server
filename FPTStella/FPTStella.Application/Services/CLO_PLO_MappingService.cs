using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.CLO_PLO_Mappings;
using FPTStella.Contracts.DTOs.CLOs;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class CLO_PLO_MappingService : ICLO_PLO_MappingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICLO_PLO_MappingRepository _mappingRepository;

        public CLO_PLO_MappingService(IUnitOfWork unitOfWork, ICLO_PLO_MappingRepository mappingRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _mappingRepository = mappingRepository ?? throw new ArgumentNullException(nameof(mappingRepository));
        }

        public async Task CreateMappingAsync(CreateCLO_PLO_MappingDto createMappingDto)
        {
            if (await _mappingRepository.IsMappingExistedAsync(createMappingDto.CloId, createMappingDto.PloId))
            {
                throw new InvalidOperationException("Mapping between CLO and PLO already exists.");
            }

            var mapping = new CLO_PLO_Mapping
            {
                CloId = createMappingDto.CloId,
                PloId = createMappingDto.PloId,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _mappingRepository.InsertAsync(mapping);
            await _unitOfWork.SaveAsync();
        }

        public async Task<CreateCLO_PLO_MappingBatchDto> CreateMappingBatchAsync(CreateCLO_PLO_MappingBatchDto createMappingBatchDto)
        {
            if (createMappingBatchDto.Mappings == null || !createMappingBatchDto.Mappings.Any())
            {
                throw new ArgumentException("No mappings provided for batch creation.");
            }

            var mappingsToInsert = new List<CLO_PLO_Mapping>();
            var createdMappings = new List<CreateCLO_PLO_MappingDto>();
            var now = DateTime.UtcNow;

            var cloRepository = _unitOfWork.Repository<CLOs>();
            var ploRepository = _unitOfWork.Repository<PLOs>();

            // Get all distinct CLO IDs and PLO IDs to validate in batches
            var distinctCloIds = createMappingBatchDto.Mappings.Select(m => m.CloId).Distinct().ToList();
            var distinctPloIds = createMappingBatchDto.Mappings.Select(m => m.PloId).Distinct().ToList();

            // Retrieve existing CLOs and PLOs to validate against
            var existingCLOs = new List<CLOs>();
            var existingPLOs = new List<PLOs>();

            foreach (var cloId in distinctCloIds)
            {
                var clo = await cloRepository.GetByIdAsync(cloId.ToString());
                if (clo != null)
                {
                    existingCLOs.Add(clo);
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
                if (await _mappingRepository.IsMappingExistedAsync(mappingDto.CloId, mappingDto.PloId))
                {
                    continue;
                }

                // Validate CLO and PLO existence
                var cloExists = existingCLOs.Any(c => c.Id == mappingDto.CloId);
                var ploExists = existingPLOs.Any(p => p.Id == mappingDto.PloId);

                if (!cloExists || !ploExists)
                {
                    continue;
                }

                var mapping = new CLO_PLO_Mapping
                {
                    CloId = mappingDto.CloId,
                    PloId = mappingDto.PloId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                };
                mappingsToInsert.Add(mapping);
                createdMappings.Add(new CreateCLO_PLO_MappingDto
                {
                    CloId = mappingDto.CloId,
                    PloId = mappingDto.PloId
                });
            }

            if (mappingsToInsert.Any())
            {
                await _mappingRepository.InsertManyAsync(mappingsToInsert);
                await _unitOfWork.SaveAsync();
            }

            // Return only the mappings that were successfully created
            return new CreateCLO_PLO_MappingBatchDto
            {
                Mappings = createdMappings
            };
        }

        public async Task<List<Guid>> GetPloIdsByCloIdAsync(Guid cloId)
        {
            return await _mappingRepository.GetPloIdsByCloIdAsync(cloId);
        }

        public async Task<List<Guid>> GetCloIdsByPloIdAsync(Guid ploId)
        {
            return await _mappingRepository.GetCloIdsByPloIdAsync(ploId);
        }

        public async Task<bool> IsMappingExistedAsync(Guid cloId, Guid ploId)
        {
            return await _mappingRepository.IsMappingExistedAsync(cloId, ploId);
        }

        public async Task DeleteMappingsByCloIdAsync(Guid cloId)
        {
            await _mappingRepository.DeleteMappingsByCloIdAsync(cloId);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteMappingsByPloIdAsync(Guid ploId)
        {
            await _mappingRepository.DeleteMappingsByPloIdAsync(ploId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<CLOWithDetailsDto>> GetCLOsWithDetailsByPloIdAsync(Guid ploId)
        {
            var cloDetailsWithName = await _mappingRepository.GetCLOsWithDetailsByPloIdAsync(ploId);

            return cloDetailsWithName.Select(c => new CLOWithDetailsDto
            {
                Id = c.Id,
                Details = c.Details
            }).ToList();
        }

        public async Task<UpdateCLO_PLO_MappingResultDto> UpdateMappingsAsync(UpdateCLO_PLO_MappingBatchDto updateMappingBatchDto)
        {
            if (updateMappingBatchDto.Mappings == null || !updateMappingBatchDto.Mappings.Any())
            {
                throw new ArgumentException("No mappings provided for update.");
            }

            var now = DateTime.UtcNow;
            var mappingsToUpdate = new List<CLO_PLO_Mapping>();
            var updatedMappings = new List<UpdateCLO_PLO_MappingDto>();
            var failedMappings = new List<(UpdateCLO_PLO_MappingDto Mapping, string Reason)>();

            // Validate CLO and PLO existence in batch
            var allCloIds = updateMappingBatchDto.Mappings
                .SelectMany(m => new[] { m.CloId, m.NewCloId })
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

            var cloRepository = _unitOfWork.Repository<CLOs>();
            var ploRepository = _unitOfWork.Repository<PLOs>();
            var existingCLOs = new Dictionary<Guid, bool>();
            var existingPLOs = new Dictionary<Guid, bool>();

            foreach (var cloId in allCloIds)
            {
                var clo = await cloRepository.GetByIdAsync(cloId.ToString());
                existingCLOs[cloId] = clo != null;
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
                    var existingMapping = await _mappingRepository.GetMappingAsync(mappingDto.CloId, mappingDto.PloId);
                    if (existingMapping == null)
                    {
                        failedMappings.Add((mappingDto, $"Mapping between CLO ID {mappingDto.CloId} and PLO ID {mappingDto.PloId} does not exist."));
                        continue;
                    }

                    // Validate new CLO ID if provided
                    if (mappingDto.NewCloId.HasValue && mappingDto.NewCloId.Value != Guid.Empty)
                    {
                        if (!existingCLOs.TryGetValue(mappingDto.NewCloId.Value, out var cloExists) || !cloExists)
                        {
                            failedMappings.Add((mappingDto, $"New CLO with ID {mappingDto.NewCloId} does not exist."));
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
                    if ((mappingDto.NewCloId.HasValue || mappingDto.NewPloId.HasValue) &&
                        await _mappingRepository.IsMappingExistedAsync(
                            mappingDto.NewCloId ?? mappingDto.CloId,
                            mappingDto.NewPloId ?? mappingDto.PloId))
                    {
                        failedMappings.Add((mappingDto, $"A mapping between CLO ID {mappingDto.NewCloId ?? mappingDto.CloId} and PLO ID {mappingDto.NewPloId ?? mappingDto.PloId} already exists."));
                        continue;
                    }

                    // Create updated mapping
                    var updatedMapping = new CLO_PLO_Mapping
                    {
                        Id = existingMapping.Id,
                        CloId = mappingDto.NewCloId ?? existingMapping.CloId,
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

            return new UpdateCLO_PLO_MappingResultDto
            {
                UpdatedMappings = updatedMappings,
                FailedMappings = failedMappings.Select(f => new FailedMappingDto
                {
                    Mapping = f.Mapping,
                    Reason = f.Reason
                }).ToList()
            };
        }

        public async Task UpdateCloPlaMappingAsync(PatchCloPloMappingDto dto)
        {
            if (dto == null || dto.PloId == Guid.Empty)
                throw new ArgumentException("Invalid PLO ID");

            // Delete old mappings
            await _mappingRepository.DeleteMappingsByPloIdAsync(dto.PloId);

            // Create new mappings if there are any CLO IDs provided
            if (dto.CloIds != null && dto.CloIds.Any())
            {
                var now = DateTime.UtcNow;
                var newMappings = dto.CloIds.Select(cloId => new CLO_PLO_Mapping
                {
                    Id = Guid.NewGuid(),
                    CloId = cloId,
                    PloId = dto.PloId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                }).ToList();

                await _mappingRepository.AddManyAsync(newMappings);
            }
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCloMappingAsync(PatchCloMappingDto dto)
        {
            if (dto == null || dto.CloId == Guid.Empty)
                throw new ArgumentException("Invalid CLO ID");

            // Delete old mappings
            await _mappingRepository.DeleteMappingsByCloIdAsync(dto.CloId);

            // Create new mappings if there are any PLO IDs provided
            if (dto.PloIds != null && dto.PloIds.Any())
            {
                var now = DateTime.UtcNow;
                var newMappings = dto.PloIds.Select(ploId => new CLO_PLO_Mapping
                {
                    Id = Guid.NewGuid(),
                    CloId = dto.CloId,
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
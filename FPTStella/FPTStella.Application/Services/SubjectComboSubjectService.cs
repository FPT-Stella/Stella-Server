using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.SubjectComboSubjects;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class SubjectComboSubjectService : ISubjectComboSubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubjectComboSubjectRepository _subjectComboSubjectRepository;
        public SubjectComboSubjectService(IUnitOfWork unitOfWork, ISubjectComboSubjectRepository subjectComboSubjectRepository )
        {
            _unitOfWork = unitOfWork;
            _subjectComboSubjectRepository = subjectComboSubjectRepository;
        }
        /// <summary>
        /// Maps a SubjectComboSubjects entity to its DTO representation
        /// </summary>
        private static SubjectComboSubjectDto MapToDto(SubjectComboSubjects entity)
        {
            return new SubjectComboSubjectDto
            {
                Id = entity.Id,
                SubjectComboId = entity.SubjectComboId,
                SubjectId = entity.SubjectId
            };
        }

        /// <summary>
        /// Creates a new mapping between a subject combo and a subject
        /// </summary>
        public async Task<SubjectComboSubjectDto> CreateMappingAsync(CreateSubjectComboSubjectDto createDto)
        {
            // Check if mapping already exists
            if (await _subjectComboSubjectRepository.IsMappingExistedAsync(createDto.SubjectComboId, createDto.SubjectId))
            {
                throw new InvalidOperationException($"A mapping between subject combo ID {createDto.SubjectComboId} and subject ID {createDto.SubjectId} already exists.");
            }

            var mapping = new SubjectComboSubjects
            {
                SubjectComboId = createDto.SubjectComboId,
                SubjectId = createDto.SubjectId,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _subjectComboSubjectRepository.InsertAsync(mapping);
            await _unitOfWork.SaveAsync();

            return MapToDto(mapping);
        }

        /// <summary>
        /// Gets a mapping by its ID
        /// </summary>
        public async Task<SubjectComboSubjectDto> GetMappingByIdAsync(Guid id)
        {
            var mapping = await _subjectComboSubjectRepository.GetByIdAsync(id.ToString());

            if (mapping == null || mapping.DelFlg)
            {
                throw new KeyNotFoundException($"Subject combo subject mapping with ID {id} not found.");
            }

            return MapToDto(mapping);
        }

        /// <summary>
        /// Gets all mappings for a specific subject combo
        /// </summary>
        public async Task<List<SubjectComboSubjectDto>> GetMappingsBySubjectComboIdAsync(Guid subjectComboId)
        {
            var mappings = await _subjectComboSubjectRepository.GetBySubjectComboIdAsync(subjectComboId);
            return mappings.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Gets all mappings for a specific subject
        /// </summary>
        public async Task<List<SubjectComboSubjectDto>> GetMappingsBySubjectIdAsync(Guid subjectId)
        {
            var mappings = await _subjectComboSubjectRepository.GetBySubjectIdAsync(subjectId);
            return mappings.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Gets all mappings
        /// </summary>
        public async Task<List<SubjectComboSubjectDto>> GetAllMappingsAsync()
        {
            var mappings = await _subjectComboSubjectRepository.GetAllAsync();
            return mappings.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Deletes a mapping by its ID
        /// </summary>
        public async Task<bool> DeleteMappingAsync(Guid id)
        {
            try
            {
                await _subjectComboSubjectRepository.DeleteAsync(id.ToString());
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete mapping: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes all mappings for a specific subject combo
        /// </summary>
        public async Task<bool> DeleteMappingsBySubjectComboIdAsync(Guid subjectComboId)
        {
            try
            {
                await _subjectComboSubjectRepository.DeleteMappingsBySubjectComboIdAsync(subjectComboId);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete mappings for subject combo: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes all mappings for a specific subject
        /// </summary>
        public async Task<bool> DeleteMappingsBySubjectIdAsync(Guid subjectId)
        {
            try
            {
                await _subjectComboSubjectRepository.DeleteMappingsBySubjectIdAsync(subjectId);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete mappings for subject: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all subject IDs associated with a specific subject combo
        /// </summary>
        public async Task<List<Guid>> GetSubjectIdsBySubjectComboIdAsync(Guid subjectComboId)
        {
            return await _subjectComboSubjectRepository.GetSubjectIdsBySubjectComboIdAsync(subjectComboId);
        }

        /// <summary>
        /// Gets all subject combo IDs that contain a specific subject
        /// </summary>
        public async Task<List<Guid>> GetSubjectComboIdsBySubjectIdAsync(Guid subjectId)
        {
            return await _subjectComboSubjectRepository.GetSubjectComboIdsBySubjectIdAsync(subjectId);
        }

        /// <summary>
        /// Checks if a mapping between a subject combo and a subject already exists
        /// </summary>
        public async Task<bool> IsMappingExistedAsync(Guid subjectComboId, Guid subjectId)
        {
            return await _subjectComboSubjectRepository.IsMappingExistedAsync(subjectComboId, subjectId);
        }

        /// <summary>
        /// Searches for subject combo subject mappings with pagination
        /// </summary>
        public async Task<PagedResult<SubjectComboSubjectDto>> SearchMappingsAsync(
            Guid? subjectComboId = null,
            Guid? subjectId = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _subjectComboSubjectRepository.SearchMappingsAsync(subjectComboId, subjectId, paginationParams);

            return new PagedResult<SubjectComboSubjectDto>
            {
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result.Items.Select(MapToDto)
            };
        }
        // Add to the SubjectComboSubjectService.cs
        /// <summary>
        /// Creates multiple mappings between subject combos and subjects in a batch operation
        /// </summary>
        /// <param name="batchDto">The batch of mappings to create</param>
        /// <returns>The successfully created mappings</returns>
        public async Task<CreateSubjectComboSubjectBatchDto> CreateMappingBatchAsync(CreateSubjectComboSubjectBatchDto batchDto)
        {
            if (batchDto.Mappings == null || !batchDto.Mappings.Any())
            {
                throw new ArgumentException("No mappings provided for batch creation.");
            }

            var mappingsToInsert = new List<SubjectComboSubjects>();
            var createdMappings = new List<CreateSubjectComboSubjectDto>();
            var now = DateTime.UtcNow;

            var subjectComboRepository = _unitOfWork.Repository<SubjectCombo>();
            var subjectRepository = _unitOfWork.Repository<Subjects>();

            // Get all distinct IDs to validate in batches
            var distinctSubjectComboIds = batchDto.Mappings.Select(m => m.SubjectComboId).Distinct().ToList();
            var distinctSubjectIds = batchDto.Mappings.Select(m => m.SubjectId).Distinct().ToList();

            // Retrieve existing records to validate against
            var existingSubjectCombos = new List<SubjectCombo>();
            var existingSubjects = new List<Subjects>();

            foreach (var id in distinctSubjectComboIds)
            {
                var combo = await subjectComboRepository.GetByIdAsync(id.ToString());
                if (combo != null && !combo.DelFlg)
                {
                    existingSubjectCombos.Add(combo);
                }
            }

            foreach (var id in distinctSubjectIds)
            {
                var subject = await subjectRepository.GetByIdAsync(id.ToString());
                if (subject != null && !subject.DelFlg)
                {
                    existingSubjects.Add(subject);
                }
            }

            foreach (var mappingDto in batchDto.Mappings)
            {
                // Skip if mapping already exists
                if (await _subjectComboSubjectRepository.IsMappingExistedAsync(mappingDto.SubjectComboId, mappingDto.SubjectId))
                {
                    continue;
                }

                // Validate existence
                var comboExists = existingSubjectCombos.Any(c => c.Id == mappingDto.SubjectComboId);
                var subjectExists = existingSubjects.Any(s => s.Id == mappingDto.SubjectId);

                if (!comboExists || !subjectExists)
                {
                    continue; // Skip invalid mappings
                }

                var mapping = new SubjectComboSubjects
                {
                    SubjectComboId = mappingDto.SubjectComboId,
                    SubjectId = mappingDto.SubjectId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                };

                mappingsToInsert.Add(mapping);
                createdMappings.Add(new CreateSubjectComboSubjectDto
                {
                    SubjectComboId = mappingDto.SubjectComboId,
                    SubjectId = mappingDto.SubjectId
                });
            }

            if (mappingsToInsert.Any())
            {
                await _subjectComboSubjectRepository.AddManyAsync(mappingsToInsert);
                await _unitOfWork.SaveAsync();
            }

            // Return only the mappings that were successfully created
            return new CreateSubjectComboSubjectBatchDto
            {
                Mappings = createdMappings
            };
        }

        /// <summary>
        /// Updates multiple mappings between subject combos and subjects in a batch operation
        /// </summary>
        /// <param name="batchDto">The batch of mappings to update</param>
        /// <returns>Result containing information about successful and failed updates</returns>
        public async Task<UpdateSubjectComboSubjectResultDto> UpdateMappingsBatchAsync(UpdateSubjectComboSubjectBatchDto batchDto)
        {
            if (batchDto.Mappings == null || !batchDto.Mappings.Any())
            {
                throw new ArgumentException("No mappings provided for update.");
            }

            var now = DateTime.UtcNow;
            var mappingsToUpdate = new List<SubjectComboSubjects>();
            var updatedMappings = new List<UpdateSubjectComboSubjectDto>();
            var failedMappings = new List<(UpdateSubjectComboSubjectDto Mapping, string Reason)>();

            // Validate subject combo and subject existence in batch
            var allSubjectComboIds = batchDto.Mappings
                .SelectMany(m => new[] { m.SubjectComboId, m.NewSubjectComboId })
                .Where(id => id.HasValue && id.Value != Guid.Empty)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var allSubjectIds = batchDto.Mappings
                .SelectMany(m => new[] { m.SubjectId, m.NewSubjectId })
                .Where(id => id.HasValue && id.Value != Guid.Empty)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var subjectComboRepository = _unitOfWork.Repository<SubjectCombo>();
            var subjectRepository = _unitOfWork.Repository<Subjects>();

            var existingSubjectCombos = new Dictionary<Guid, bool>();
            var existingSubjects = new Dictionary<Guid, bool>();

            foreach (var id in allSubjectComboIds)
            {
                var combo = await subjectComboRepository.GetByIdAsync(id.ToString());
                existingSubjectCombos[id] = combo != null && !combo.DelFlg;
            }

            foreach (var id in allSubjectIds)
            {
                var subject = await subjectRepository.GetByIdAsync(id.ToString());
                existingSubjects[id] = subject != null && !subject.DelFlg;
            }

            foreach (var mappingDto in batchDto.Mappings)
            {
                try
                {
                    // Validate the existing mapping
                    var existingMapping = await _subjectComboSubjectRepository.GetMappingAsync(mappingDto.SubjectComboId, mappingDto.SubjectId);
                    if (existingMapping == null)
                    {
                        failedMappings.Add((mappingDto, $"Mapping between subject combo ID {mappingDto.SubjectComboId} and subject ID {mappingDto.SubjectId} does not exist."));
                        continue;
                    }

                    // Validate new subject combo ID if provided
                    if (mappingDto.NewSubjectComboId.HasValue && mappingDto.NewSubjectComboId.Value != Guid.Empty)
                    {
                        if (!existingSubjectCombos.TryGetValue(mappingDto.NewSubjectComboId.Value, out var comboExists) || !comboExists)
                        {
                            failedMappings.Add((mappingDto, $"New subject combo with ID {mappingDto.NewSubjectComboId} does not exist."));
                            continue;
                        }
                    }

                    // Validate new subject ID if provided
                    if (mappingDto.NewSubjectId.HasValue && mappingDto.NewSubjectId.Value != Guid.Empty)
                    {
                        if (!existingSubjects.TryGetValue(mappingDto.NewSubjectId.Value, out var subjectExists) || !subjectExists)
                        {
                            failedMappings.Add((mappingDto, $"New subject with ID {mappingDto.NewSubjectId} does not exist."));
                            continue;
                        }
                    }

                    // Check if the new mapping already exists
                    if ((mappingDto.NewSubjectComboId.HasValue || mappingDto.NewSubjectId.HasValue) &&
                        await _subjectComboSubjectRepository.IsMappingExistedAsync(
                            mappingDto.NewSubjectComboId ?? mappingDto.SubjectComboId,
                            mappingDto.NewSubjectId ?? mappingDto.SubjectId))
                    {
                        failedMappings.Add((mappingDto, $"A mapping between subject combo ID {mappingDto.NewSubjectComboId ?? mappingDto.SubjectComboId} and subject ID {mappingDto.NewSubjectId ?? mappingDto.SubjectId} already exists."));
                        continue;
                    }

                    // Create updated mapping
                    var updatedMapping = new SubjectComboSubjects
                    {
                        Id = existingMapping.Id,
                        SubjectComboId = mappingDto.NewSubjectComboId ?? existingMapping.SubjectComboId,
                        SubjectId = mappingDto.NewSubjectId ?? existingMapping.SubjectId,
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
                await _subjectComboSubjectRepository.UpdateManyAsync(mappingsToUpdate);
                await _unitOfWork.SaveAsync();
            }

            return new UpdateSubjectComboSubjectResultDto
            {
                UpdatedMappings = updatedMappings,
                FailedMappings = failedMappings.Select(f => new FailedMappingDto
                {
                    Mapping = f.Mapping,
                    Reason = f.Reason
                }).ToList()
            };
        }

        /// <summary>
        /// Updates all subject mappings for a specific subject combo by replacing existing mappings
        /// </summary>
        /// <param name="dto">The patch DTO containing subject combo ID and new subject IDs</param>
        public async Task UpdateSubjectComboMappingAsync(PatchSubjectComboMappingDto dto)
        {
            if (dto == null || dto.SubjectComboId == Guid.Empty)
                throw new ArgumentException("Invalid subject combo ID");

            // Delete old mappings
            await _subjectComboSubjectRepository.DeleteMappingsBySubjectComboIdAsync(dto.SubjectComboId);

            // Create new ones if any
            if (dto.SubjectIds != null && dto.SubjectIds.Any())
            {
                var now = DateTime.UtcNow;
                var newMappings = dto.SubjectIds.Select(subjectId => new SubjectComboSubjects
                {
                    Id = Guid.NewGuid(),
                    SubjectComboId = dto.SubjectComboId,
                    SubjectId = subjectId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                }).ToList();

                await _subjectComboSubjectRepository.AddManyAsync(newMappings);
            }

            await _unitOfWork.SaveAsync();
        }
        /// <summary>
        /// Updates all subject combo mappings for a specific subject by replacing existing mappings
        /// </summary>
        /// <param name="dto">The patch DTO containing subject ID and new subject combo IDs</param>
        public async Task UpdateSubjectMappingAsync(PatchSubjectMappingDto dto)
        {
            if (dto == null || dto.SubjectId == Guid.Empty)
                throw new ArgumentException("Invalid subject ID");

            // Delete old mappings
            await _subjectComboSubjectRepository.DeleteMappingsBySubjectIdAsync(dto.SubjectId);

            // Create new ones if any
            if (dto.SubjectComboIds != null && dto.SubjectComboIds.Any())
            {
                var now = DateTime.UtcNow;
                var newMappings = dto.SubjectComboIds.Select(subjectComboId => new SubjectComboSubjects
                {
                    Id = Guid.NewGuid(),
                    SubjectId = dto.SubjectId,
                    SubjectComboId = subjectComboId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                }).ToList();

                await _subjectComboSubjectRepository.AddManyAsync(newMappings);
            }

            await _unitOfWork.SaveAsync();
        }
    }
}

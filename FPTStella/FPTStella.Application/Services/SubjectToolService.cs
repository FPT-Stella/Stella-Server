using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.SubjectTools;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class SubjectToolService : ISubjectToolService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubjectToolRepository _subjectToolRepository;

        /// <summary>
        /// Initializes a new instance of the SubjectToolService class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        /// <param name="subjectToolRepository">The subject-tool mapping repository</param>
        public SubjectToolService(IUnitOfWork unitOfWork, ISubjectToolRepository subjectToolRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _subjectToolRepository = subjectToolRepository ?? throw new ArgumentNullException(nameof(subjectToolRepository));
        }

        /// <summary>
        /// Creates a new mapping between Subject and Tool.
        /// </summary>
        /// <param name="createMappingDto">The DTO containing mapping creation data</param>
        /// <exception cref="InvalidOperationException">Thrown when a mapping already exists</exception>
        public async Task CreateMappingAsync(CreateSubjectToolDto createMappingDto)
        {
            if (await _subjectToolRepository.IsMappingExistedAsync(createMappingDto.SubjectId, createMappingDto.ToolId))
            {
                throw new InvalidOperationException("Mapping between Subject and Tool already exists.");
            }

            var mapping = new SubjectTool
            {
                SubjectId = createMappingDto.SubjectId,
                ToolId = createMappingDto.ToolId,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _subjectToolRepository.InsertAsync(mapping);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Creates multiple mappings between Subjects and Tools in a single operation and returns the created mappings.
        /// </summary>
        /// <param name="createMappingBatchDto">The DTO containing multiple mapping creation data</param>
        /// <returns>The DTO with successfully created mappings</returns>
        public async Task<CreateSubjectToolBatchDto> CreateMappingBatchAsync(CreateSubjectToolBatchDto createMappingBatchDto)
        {
            if (createMappingBatchDto.Mappings == null || !createMappingBatchDto.Mappings.Any())
            {
                throw new ArgumentException("No mappings provided for batch creation.");
            }

            var mappingsToInsert = new List<SubjectTool>();
            var createdMappings = new List<CreateSubjectToolDto>();
            var invalidMappings = new List<(CreateSubjectToolDto Mapping, string Reason)>();
            var now = DateTime.UtcNow;

            var subjectRepository = _unitOfWork.Repository<Subjects>();
            var toolRepository = _unitOfWork.Repository<Tools>();

            // Get all distinct Subject IDs and Tool IDs to validate in batches
            var distinctSubjectIds = createMappingBatchDto.Mappings.Select(m => m.SubjectId).Distinct().ToList();
            var distinctToolIds = createMappingBatchDto.Mappings.Select(m => m.ToolId).Distinct().ToList();

            // Retrieve existing Subjects and Tools to validate against
            var existingSubjects = new List<Subjects>();
            var existingTools = new List<Tools>();

            foreach (var subjectId in distinctSubjectIds)
            {
                var subject = await subjectRepository.GetByIdAsync(subjectId.ToString());
                if (subject != null)
                {
                    existingSubjects.Add(subject);
                }
            }

            foreach (var toolId in distinctToolIds)
            {
                var tool = await toolRepository.GetByIdAsync(toolId.ToString());
                if (tool != null)
                {
                    existingTools.Add(tool);
                }
            }

            foreach (var mappingDto in createMappingBatchDto.Mappings)
            {
                // Skip if mapping already exists
                if (await _subjectToolRepository.IsMappingExistedAsync(mappingDto.SubjectId, mappingDto.ToolId))
                {
                    continue;
                }

                // Validate Subject and Tool existence
                var subjectExists = existingSubjects.Any(s => s.Id == mappingDto.SubjectId);
                var toolExists = existingTools.Any(t => t.Id == mappingDto.ToolId);

                if (!subjectExists)
                {
                    invalidMappings.Add((mappingDto, $"Subject with ID {mappingDto.SubjectId} does not exist"));
                    continue;
                }

                if (!toolExists)
                {
                    invalidMappings.Add((mappingDto, $"Tool with ID {mappingDto.ToolId} does not exist"));
                    continue;
                }

                var mapping = new SubjectTool
                {
                    SubjectId = mappingDto.SubjectId,
                    ToolId = mappingDto.ToolId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                };

                mappingsToInsert.Add(mapping);
                createdMappings.Add(new CreateSubjectToolDto
                {
                    SubjectId = mappingDto.SubjectId,
                    ToolId = mappingDto.ToolId
                });
            }

            if (mappingsToInsert.Any())
            {
                await _subjectToolRepository.AddManyAsync(mappingsToInsert);
                await _unitOfWork.SaveAsync();
            }

            // Return only the mappings that were successfully created
            return new CreateSubjectToolBatchDto
            {
                Mappings = createdMappings
            };
        }

        /// <summary>
        /// Gets Tool IDs associated with a specific Subject.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <returns>List of Tool IDs</returns>
        public async Task<List<Guid>> GetToolIdsBySubjectIdAsync(Guid subjectId)
        {
            return await _subjectToolRepository.GetToolIdsBySubjectIdAsync(subjectId);
        }

        /// <summary>
        /// Gets Subject IDs associated with a specific Tool.
        /// </summary>
        /// <param name="toolId">The Tool ID</param>
        /// <returns>List of Subject IDs</returns>
        public async Task<List<Guid>> GetSubjectIdsByToolIdAsync(Guid toolId)
        {
            return await _subjectToolRepository.GetSubjectIdsByToolIdAsync(toolId);
        }

        /// <summary>
        /// Checks if a mapping between specific Subject and Tool exists.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <param name="toolId">The Tool ID</param>
        /// <returns>True if the mapping exists, false otherwise</returns>
        public async Task<bool> IsMappingExistedAsync(Guid subjectId, Guid toolId)
        {
            return await _subjectToolRepository.IsMappingExistedAsync(subjectId, toolId);
        }

        /// <summary>
        /// Deletes all mappings associated with a specific Subject.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        public async Task DeleteMappingsBySubjectIdAsync(Guid subjectId)
        {
            await _subjectToolRepository.DeleteMappingsBySubjectIdAsync(subjectId);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Deletes all mappings associated with a specific Tool.
        /// </summary>
        /// <param name="toolId">The Tool ID</param>
        public async Task DeleteMappingsByToolIdAsync(Guid toolId)
        {
            await _subjectToolRepository.DeleteMappingsByToolIdAsync(toolId);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Gets Tools with names associated with a specific Subject.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <returns>List of Tools with names</returns>
        public async Task<List<ToolWithNameDto>> GetToolsWithNameBySubjectIdAsync(Guid subjectId)
        {
            var toolsWithName = await _subjectToolRepository.GetToolsWithNameBySubjectIdAsync(subjectId);

            return toolsWithName.Select(t => new ToolWithNameDto
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();
        }

        /// <summary>
        /// Updates multiple mappings between Subjects and Tools.
        /// </summary>
        /// <param name="updateMappingBatchDto">The batch update DTO</param>
        /// <returns>Result of the update operation</returns>
        public async Task<UpdateSubjectToolResultDto> UpdateMappingsAsync(UpdateSubjectToolBatchDto updateMappingBatchDto)
        {
            if (updateMappingBatchDto.Mappings == null || !updateMappingBatchDto.Mappings.Any())
            {
                throw new ArgumentException("No mappings provided for update.");
            }

            var now = DateTime.UtcNow;
            var mappingsToUpdate = new List<SubjectTool>();
            var updatedMappings = new List<UpdateSubjectToolDto>();
            var failedMappings = new List<(UpdateSubjectToolDto Mapping, string Reason)>();

            // Validate Subject and Tool existence in batch
            var allSubjectIds = updateMappingBatchDto.Mappings
                .SelectMany(m => new[] { m.SubjectId, m.NewSubjectId })
                .Where(id => id.HasValue && id.Value != Guid.Empty)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var allToolIds = updateMappingBatchDto.Mappings
                .SelectMany(m => new[] { m.ToolId, m.NewToolId })
                .Where(id => id.HasValue && id.Value != Guid.Empty)
                .Select(id => id!.Value)
                .Distinct()
                .ToList();

            var subjectRepository = _unitOfWork.Repository<Subjects>();
            var toolRepository = _unitOfWork.Repository<Tools>();

            var existingSubjects = new Dictionary<Guid, bool>();
            var existingTools = new Dictionary<Guid, bool>();

            foreach (var subjectId in allSubjectIds)
            {
                var subject = await subjectRepository.GetByIdAsync(subjectId.ToString());
                existingSubjects[subjectId] = subject != null;
            }

            foreach (var toolId in allToolIds)
            {
                var tool = await toolRepository.GetByIdAsync(toolId.ToString());
                existingTools[toolId] = tool != null;
            }

            foreach (var mappingDto in updateMappingBatchDto.Mappings)
            {
                try
                {
                    // Validate the existing mapping
                    var existingMapping = await _subjectToolRepository.GetMappingAsync(mappingDto.SubjectId, mappingDto.ToolId);
                    if (existingMapping == null)
                    {
                        failedMappings.Add((mappingDto, $"Mapping between Subject ID {mappingDto.SubjectId} and Tool ID {mappingDto.ToolId} does not exist."));
                        continue;
                    }

                    // Validate new Subject ID if provided
                    if (mappingDto.NewSubjectId.HasValue && mappingDto.NewSubjectId.Value != Guid.Empty)
                    {
                        if (!existingSubjects.TryGetValue(mappingDto.NewSubjectId.Value, out var subjectExists) || !subjectExists)
                        {
                            failedMappings.Add((mappingDto, $"New Subject with ID {mappingDto.NewSubjectId} does not exist."));
                            continue;
                        }
                    }

                    // Validate new Tool ID if provided
                    if (mappingDto.NewToolId.HasValue && mappingDto.NewToolId.Value != Guid.Empty)
                    {
                        if (!existingTools.TryGetValue(mappingDto.NewToolId.Value, out var toolExists) || !toolExists)
                        {
                            failedMappings.Add((mappingDto, $"New Tool with ID {mappingDto.NewToolId} does not exist."));
                            continue;
                        }
                    }

                    // Check if the new mapping already exists
                    if ((mappingDto.NewSubjectId.HasValue || mappingDto.NewToolId.HasValue) &&
                        await _subjectToolRepository.IsMappingExistedAsync(
                            mappingDto.NewSubjectId ?? mappingDto.SubjectId,
                            mappingDto.NewToolId ?? mappingDto.ToolId))
                    {
                        failedMappings.Add((mappingDto, $"A mapping between Subject ID {mappingDto.NewSubjectId ?? mappingDto.SubjectId} and Tool ID {mappingDto.NewToolId ?? mappingDto.ToolId} already exists."));
                        continue;
                    }

                    // Create updated mapping
                    var updatedMapping = new SubjectTool
                    {
                        Id = existingMapping.Id,
                        SubjectId = mappingDto.NewSubjectId ?? existingMapping.SubjectId,
                        ToolId = mappingDto.NewToolId ?? existingMapping.ToolId,
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
                await _subjectToolRepository.UpdateManyAsync(mappingsToUpdate);
                await _unitOfWork.SaveAsync();
            }

            return new UpdateSubjectToolResultDto
            {
                UpdatedMappings = updatedMappings,
                FailedMappings = failedMappings.Select(f => new FailedSubjectToolMappingDto
                {
                    Mapping = f.Mapping,
                    Reason = f.Reason
                }).ToList()
            };
        }

        /// <summary>
        /// Updates Subject-Tool mappings by replacing all Tools for a given Subject.
        /// </summary>
        /// <param name="dto">The patch DTO containing Subject ID and new Tool IDs</param>
        public async Task UpdateSubjectToolMappingAsync(PatchSubjectToolMappingDto dto)
        {
            if (dto == null || dto.SubjectId == Guid.Empty)
                throw new ArgumentException("Invalid Subject ID");

            // Delete old mappings
            await _subjectToolRepository.DeleteMappingsBySubjectIdAsync(dto.SubjectId);

            // Create new ones if any
            if (dto.ToolIds != null && dto.ToolIds.Any())
            {
                var now = DateTime.UtcNow;
                var newMappings = dto.ToolIds.Select(toolId => new SubjectTool
                {
                    Id = Guid.NewGuid(),
                    SubjectId = dto.SubjectId,
                    ToolId = toolId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                }).ToList();

                await _subjectToolRepository.AddManyAsync(newMappings);
            }

            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Updates Tool-Subject mappings by replacing all Subjects for a given Tool.
        /// This is the reverse operation of UpdateSubjectToolMappingAsync.
        /// </summary>
        /// <param name="dto">The patch DTO containing Tool ID and new Subject IDs</param>
        public async Task UpdateToolSubjectMappingAsync(PatchToolSubjectMappingDto dto)
        {
            if (dto == null || dto.ToolId == Guid.Empty)
                throw new ArgumentException("Invalid Tool ID");

            // Delete old mappings
            await _subjectToolRepository.DeleteMappingsByToolIdAsync(dto.ToolId);

            // Create new ones if any
            if (dto.SubjectIds != null && dto.SubjectIds.Any())
            {
                var now = DateTime.UtcNow;
                var newMappings = dto.SubjectIds.Select(subjectId => new SubjectTool
                {
                    Id = Guid.NewGuid(),
                    SubjectId = subjectId,
                    ToolId = dto.ToolId,
                    InsDate = now,
                    UpdDate = now,
                    DelFlg = false
                }).ToList();

                await _subjectToolRepository.AddManyAsync(newMappings);
            }

            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Validates that the given Subject and Tool IDs exist in the database.
        /// </summary>
        private async Task ValidateIdsExistAsync(Guid subjectId, Guid toolId)
        {
            var subjectRepository = _unitOfWork.Repository<Subjects>();
            var toolRepository = _unitOfWork.Repository<Tools>();

            var subject = await subjectRepository.GetByIdAsync(subjectId.ToString());
            if (subject == null)
            {
                throw new KeyNotFoundException($"Subject with ID {subjectId} does not exist.");
            }

            var tool = await toolRepository.GetByIdAsync(toolId.ToString());
            if (tool == null)
            {
                throw new KeyNotFoundException($"Tool with ID {toolId} does not exist.");
            }
        }
    }
}
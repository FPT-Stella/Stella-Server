using FPTStella.Contracts.DTOs.SubjectTools;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface ISubjectToolService
    {
        Task CreateMappingAsync(CreateSubjectToolDto createMappingDto);
        Task<List<Guid>> GetToolIdsBySubjectIdAsync(Guid subjectId);
        Task<List<Guid>> GetSubjectIdsByToolIdAsync(Guid toolId);
        Task<bool> IsMappingExistedAsync(Guid subjectId, Guid toolId);
        Task DeleteMappingsBySubjectIdAsync(Guid subjectId);
        Task DeleteMappingsByToolIdAsync(Guid toolId);
        Task<List<ToolWithNameDto>> GetToolsWithNameBySubjectIdAsync(Guid subjectId);
        Task<CreateSubjectToolBatchDto> CreateMappingBatchAsync(CreateSubjectToolBatchDto createMappingBatchDto);
        Task<UpdateSubjectToolResultDto> UpdateMappingsAsync(UpdateSubjectToolBatchDto updateMappingBatchDto);
        Task UpdateSubjectToolMappingAsync(PatchSubjectToolMappingDto dto);
        Task UpdateToolSubjectMappingAsync(PatchToolSubjectMappingDto dto);
    }
}
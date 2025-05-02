using FPTStella.Contracts.DTOs.CLO_PLO_Mappings;
using FPTStella.Contracts.DTOs.CLOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface ICLO_PLO_MappingService
    {
        Task CreateMappingAsync(CreateCLO_PLO_MappingDto createMappingDto);
        Task<List<Guid>> GetPloIdsByCloIdAsync(Guid cloId);
        Task<List<Guid>> GetCloIdsByPloIdAsync(Guid ploId);
        Task<bool> IsMappingExistedAsync(Guid cloId, Guid ploId);
        Task DeleteMappingsByCloIdAsync(Guid cloId);
        Task DeleteMappingsByPloIdAsync(Guid ploId);
        Task<List<CLOWithDetailsDto>> GetCLOsWithDetailsByPloIdAsync(Guid ploId);
        Task<CreateCLO_PLO_MappingBatchDto> CreateMappingBatchAsync(CreateCLO_PLO_MappingBatchDto createMappingBatchDto);
        Task<UpdateCLO_PLO_MappingResultDto> UpdateMappingsAsync(UpdateCLO_PLO_MappingBatchDto updateMappingBatchDto);
        Task UpdateCloPlaMappingAsync(PatchCloPloMappingDto dto);
        Task UpdateCloMappingAsync(PatchCloMappingDto dto);
    }
}

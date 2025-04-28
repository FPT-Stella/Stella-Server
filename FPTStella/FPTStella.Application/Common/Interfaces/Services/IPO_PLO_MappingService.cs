using FPTStella.Contracts.DTOs.PO_PLO_Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IPO_PLO_MappingService
    {
        Task CreateMappingAsync(CreatePO_PLO_MappingDto createMappingDto);
        Task<List<Guid>> GetPloIdsByPoIdAsync(Guid poId);
        Task<List<Guid>> GetPoIdsByPloIdAsync(Guid ploId);
        Task<bool> IsMappingExistedAsync(Guid poId, Guid ploId);
        Task DeleteMappingsByPoIdAsync(Guid poId);
        Task DeleteMappingsByPloIdAsync(Guid ploId);
        Task<List<POWithNameDto>> GetPOsWithNameByPloIdAsync(Guid ploId);
        Task<CreatePO_PLO_MappingBatchDto> CreateMappingBatchAsync(CreatePO_PLO_MappingBatchDto createMappingBatchDto);

    }
}

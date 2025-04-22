using FPTStella.Contracts.DTOs.PLOs;
using FPTStella.Contracts.DTOs.POs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IPLOService
    {
        Task<PLOsDto> CreatePLOAsync(CreatePLOsDto createPLOsDto);
        Task<List<PLOsDto>> GetAllPLOsAsync();
        Task<PLOsDto> GetPLOByIdAsync(Guid id);
        Task<List<PLOsDto>> GetPLOsByCurriculumIdAsync(Guid curriculumId);
        Task<List<PLOsDto>> GetPLOsByCurriculumIdsAsync(List<Guid> curriculumIds);
        Task<bool> IsPloNameExistedAsync(Guid curriculumId, string ploName);
        Task UpdatePLOAsync(Guid id, UpdatePLOsDto updatePLOsDto);
        Task DeletePLOAsync(Guid id);
        Task DeletePLOsByCurriculumIdAsync(Guid curriculumId);
    }
}

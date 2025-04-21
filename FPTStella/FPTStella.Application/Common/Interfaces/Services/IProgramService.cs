using FPTStella.Contracts.DTOs.Programs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IProgramService 
    {
        Task<ProgramDto> CreateProgramAsync(CreateProgramDto createProgramDto);
        Task<List<ProgramDto>> GetAllProgramsAsync();
        Task<ProgramDto> GetProgramByIdAsync(string id);
        Task<ProgramDto> GetProgramByProgramCodeAsync(string programCode);
        Task<ProgramDto> GetProgramByMajorIdAsync(Guid majorId);
        Task<ProgramDto> GetProgramByProgramNameAsync(string programName);
        Task<ProgramDto> GetProgramByMajorIdAndProgramNameAsync(Guid majorId, string programName);
        Task<ProgramDto> GetProgramByMajorIdAndProgramCodeAsync(Guid majorId, string programCode);
        Task UpdateProgramAsync(string id, UpdateProgramDto updateProgramDto);
        Task DeleteProgramAsync(string id);
    }
}

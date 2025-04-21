using FPTStella.Contracts.DTOs.Majors;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IMajorService
    {
        Task<MajorDto> CreateMajorAsync(CreateMajorDto createMajorDto);
        Task UpdateMajorAsync(Guid id, UpdateMajorDto updateMajorDto);
        Task DeleteMajorAsync(Guid id);
        Task<bool> MajorExistsAsync(string majorName);
        Task<bool> MajorExistsByIdAsync(Guid id);
        Task<List<MajorDto>> GetAllMajorsAsync();
        Task<MajorDto> GetMajorByIdAsync(Guid id);
    }
}

using FPTStella.Contracts.DTOs.CLOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface ICLOService
    {
        Task<Guid> CreateCloAsync(CreateCLODto createCloDto);
        Task<List<CLOWithDetailsDto>> GetClosBySubjectIdAsync(Guid subjectId);
        Task DeleteClosBySubjectIdAsync(Guid subjectId);
    }
}

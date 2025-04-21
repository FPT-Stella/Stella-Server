using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface IProgramRepository : IRepository<Programs>
    {
        Task<Programs?> GetByProgramCodeAsync(string programCode);
        Task<Programs?> GetByMajorIdAsync(Guid majorId);
        Task<Programs?> GetByProgramNameAsync(string programName);
        Task<Programs?> GetByMajorIdAndProgramNameAsync(Guid majorId, string programName);
        Task<Programs?> GetByMajorIdAndProgramCodeAsync(Guid majorId, string programCode);

    }
}

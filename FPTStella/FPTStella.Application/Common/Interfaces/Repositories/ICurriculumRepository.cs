using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface ICurriculumRepository : IRepository<Curriculums>
    {
        Task<Curriculums?> GetByCurriculumCodeAsync(string curriculumCode);
        Task<Curriculums?> GetByCurriculumNameAsync(string curriculumName);
        Task<List<Curriculums>> GetByProgramIdAsync(Guid programId);
        Task<bool> IsCurriculumCodeExisted(string curriculumCode);
        Task<bool> IsCurriculumNameExisted(string curriculumName);
        Task<bool> IsCurriculumCodeExisted(string curriculumCode, Guid id);
        Task<bool> IsCurriculumNameExisted(string curriculumName, Guid id);
    }
}

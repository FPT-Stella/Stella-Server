using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface ICLORepository : IRepository<CLOs>
    {
        /// <summary>
        /// Gets CLOs by Subject ID.
        /// </summary>
        /// <param name="subjectId">ID of the subject</param>
        /// <returns>List of CLOs for the subject</returns>
        Task<List<CLOs>> GetBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Checks if a CLO exists for a specific subject with the given details
        /// </summary>
        /// <param name="subjectId">Subject ID</param>
        /// <param name="cloDetails">CLO details to check</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> IsCloExistedAsync(Guid subjectId, string cloDetails);

        /// <summary>
        /// Deletes CLOs associated with a subject
        /// </summary>
        /// <param name="subjectId">Subject ID</param>
        Task DeleteClosBySubjectIdAsync(Guid subjectId);
    }
}

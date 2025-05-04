using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
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
        /// Checks if a CLO with the given name exists for a specific subject.
        /// </summary>
        /// <param name="subjectId">Subject ID</param>
        /// <param name="cloName">CLO name to check</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> IsCloNameExistedAsync(Guid subjectId, string cloName);

        /// <summary>
        /// Checks if a CLO exists for a specific subject with the given details.
        /// </summary>
        /// <param name="subjectId">Subject ID</param>
        /// <param name="cloDetails">CLO details to check</param>
        /// <returns>True if exists, false otherwise</returns>
        Task<bool> IsCloDetailsExistedAsync(Guid subjectId, string cloDetails);

        /// <summary>
        /// Gets CLOs by a list of Subject IDs.
        /// </summary>
        /// <param name="subjectIds">List of Subject IDs</param>
        /// <returns>List of CLOs</returns>
        Task<List<CLOs>> GetBySubjectIdsAsync(List<Guid> subjectIds);

        /// <summary>
        /// Deletes CLOs associated with a specific Subject ID.
        /// </summary>
        /// <param name="subjectId">Subject ID</param>
        Task DeleteBySubjectIdAsync(Guid subjectId);
    }
}
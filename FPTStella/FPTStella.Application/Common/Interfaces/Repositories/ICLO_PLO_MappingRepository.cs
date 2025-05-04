using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface ICLO_PLO_MappingRepository : IRepository<CLO_PLO_Mapping>
    {
        /// <summary>
        /// Gets CLO_PLO_Mapping by CLO ID.
        /// </summary>
        /// <param name="cloId">ID of the CLO.</param>
        /// <returns>List of CLO_PLO_Mapping.</returns>
        Task<List<CLO_PLO_Mapping>> GetByCloIdAsync(Guid cloId);

        /// <summary>
        /// Gets CLO_PLO_Mapping by PLO ID.
        /// </summary>
        /// <param name="ploId">ID of the PLO.</param>
        /// <returns>List of CLO_PLO_Mapping.</returns>
        Task<List<CLO_PLO_Mapping>> GetByPloIdAsync(Guid ploId);

        /// <summary>
        /// Checks if a mapping between CLO and PLO already exists.
        /// </summary>
        Task<bool> IsMappingExistedAsync(Guid cloId, Guid ploId);

        /// <summary>
        /// Deletes mappings by CLO ID.
        /// <param name="cloId">ID of the CLO.</param>
        /// </summary>
        Task DeleteMappingsByCloIdAsync(Guid cloId);

        /// <summary>
        /// Deletes mappings by PLO ID.
        /// <param name="ploId">ID of the PLO.</param>
        /// </summary>
        Task DeleteMappingsByPloIdAsync(Guid ploId);

        /// <summary>
        /// Gets PLO IDs linked to a CLO.
        /// <param name="cloId">ID of the CLO.</param>
        /// </summary>
        Task<List<Guid>> GetPloIdsByCloIdAsync(Guid cloId);
        /// <summary>
        /// Gets CLO IDs linked to a PLO.
        /// <param name="ploId">ID of the PLO.</param>
        /// </summary>
        Task<List<Guid>> GetCloIdsByPloIdAsync(Guid ploId);

        /// <summary>
        /// Gets CLO IDs linked to a PLO.
        /// <param name="ploId">ID of the PLO.</param>
        /// </summary>
        /// Task<List<Guid>> GetCloIdsByPloIdAsync(Guid ploId);

        /// <summary>
        /// Gets CLOs with details by PLO ID.
        /// </summary>
        Task<List<(Guid Id, string Details)>> GetCLOsWithDetailsByPloIdAsync(Guid ploId);

        /// <summary>
        /// Updates a CLO_PLO mapping.
        /// </summary>
        Task UpdateAsync(CLO_PLO_Mapping mapping);

        /// <summary>
        /// Updates multiple CLO_PLO mappings.
        /// </summary>
        Task UpdateManyAsync(IEnumerable<CLO_PLO_Mapping> mappings);

        /// <summary>
        /// Gets a specific mapping between CLO and PLO.
        /// </summary>
        Task<CLO_PLO_Mapping?> GetMappingAsync(Guid cloId, Guid ploId);

        /// <summary>
        /// Adds multiple mappings at once.
        /// </summary>
        Task AddManyAsync(IEnumerable<CLO_PLO_Mapping> mappings);
        Task<List<(Guid Id, string PloName, string CurriculumName, string Description)>> GetPLOsWithDetailsByCloIdAsync(Guid cloId);

    }
}

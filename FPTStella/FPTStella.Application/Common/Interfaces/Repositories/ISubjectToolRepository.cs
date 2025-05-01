using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface ISubjectToolRepository : IRepository<SubjectTool>
    {
        /// <summary>
        /// Gets a list of SubjectTool mappings by Subject ID.
        /// </summary>
        /// <param name="subjectId">ID of the Subject</param>
        /// <returns>List of SubjectTool mappings</returns>
        Task<List<SubjectTool>> GetBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Gets a list of SubjectTool mappings by Tool ID.
        /// </summary>
        /// <param name="toolId">ID of the Tool</param>
        /// <returns>List of SubjectTool mappings</returns>
        Task<List<SubjectTool>> GetByToolIdAsync(Guid toolId);

        /// <summary>
        /// Checks if a mapping between a specific Subject and Tool already exists.
        /// </summary>
        Task<bool> IsMappingExistedAsync(Guid subjectId, Guid toolId);

        /// <summary>
        /// Deletes mappings between Subjects and Tools by Subject ID.
        /// </summary>
        /// <param name="subjectId">ID of the Subject</param>
        Task DeleteMappingsBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Deletes mappings between Subjects and Tools by Tool ID.
        /// </summary>
        /// <param name="toolId">ID of the Tool</param>
        Task DeleteMappingsByToolIdAsync(Guid toolId);

        /// <summary>
        /// Gets a list of Tool IDs associated with a specific Subject.
        /// </summary>
        /// <param name="subjectId">ID of the Subject</param>
        Task<List<Guid>> GetToolIdsBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Gets a list of Subject IDs associated with a specific Tool.
        /// </summary>
        /// <param name="toolId">ID of the Tool</param>
        Task<List<Guid>> GetSubjectIdsByToolIdAsync(Guid toolId);

        /// <summary>
        /// Gets a list of Tools with their names associated with a specific Subject.
        /// </summary>
        /// <param name="subjectId">ID of the Subject</param>
        Task<List<(Guid Id, string Name)>> GetToolsWithNameBySubjectIdAsync(Guid subjectId);

        /// <summary>
        /// Gets a list of Subjects with their names associated with a specific Tool.
        /// </summary>
        /// <param name="toolId">ID of the Tool</param>
        Task<List<(Guid Id, string Name)>> GetSubjectsWithNameByToolIdAsync(Guid toolId);

        /// <summary>
        /// Updates a SubjectTool mapping in the database.
        /// </summary>
        /// <param name="mapping">The mapping entity to update</param>
        Task UpdateAsync(SubjectTool mapping);

        /// <summary>
        /// Updates multiple SubjectTool mappings in the database.
        /// </summary>
        /// <param name="mappings">The collection of mapping entities to update</param>
        Task UpdateManyAsync(IEnumerable<SubjectTool> mappings);

        /// <summary>
        /// Gets a specific mapping between a Subject and a Tool.
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <param name="toolId">The Tool ID</param>
        /// <returns>The mapping if it exists, otherwise null</returns>
        Task<SubjectTool?> GetMappingAsync(Guid subjectId, Guid toolId);

        /// <summary>
        /// Adds multiple SubjectTool mappings to the database in a single operation.
        /// </summary>
        /// <param name="mappings">The collection of mappings to add</param>
        Task AddManyAsync(IEnumerable<SubjectTool> mappings);
    }
}
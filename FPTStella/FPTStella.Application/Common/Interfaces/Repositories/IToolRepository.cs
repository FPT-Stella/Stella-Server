using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface IToolRepository : IRepository<Tools>
    {
        /// <summary>
        /// Gets a tool by its name
        /// </summary>
        /// <param name="toolName">The name of the tool</param>
        /// <returns>The tool if found, otherwise null</returns>
        Task<Tools?> GetByToolNameAsync(string toolName);

        /// <summary>
        /// Checks if a tool with the specified name already exists
        /// </summary>
        /// <param name="toolName">The name to check</param>
        /// <returns>True if the tool name exists, otherwise false</returns>
        Task<bool> IsToolNameExistedAsync(string toolName);

        /// <summary>
        /// Searches for tools with pagination and filtering by name or description
        /// </summary>
        /// <param name="searchTerm">Optional search term for tool name or description</param>
        /// <param name="paginationParams">Pagination parameters</param>
        /// <returns>Paginated results of tools</returns>
        Task<PagedResult<Tools>> SearchToolsAsync(
            string? searchTerm = null,
            PaginationParams? paginationParams = null);

        /// <summary>
        /// Gets all tools with optional filtering by tool name
        /// </summary>
        /// <param name="toolName">Optional tool name filter (partial match)</param>
        /// <returns>List of tools matching the criteria</returns>
        Task<List<Tools>> GetToolsByNameContainingAsync(string? toolName = null);
    }
}

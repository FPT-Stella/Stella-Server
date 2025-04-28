using FPTStella.Contracts.DTOs.Tools;
using FPTStella.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IToolService
    {
        /// <summary>
        /// Creates a new tool
        /// </summary>
        /// <param name="createDto">The data for the new tool</param>
        /// <returns>The created tool as a DTO</returns>
        Task<ToolDto> CreateToolAsync(CreateToolDto createDto);

        /// <summary>
        /// Gets a tool by its ID
        /// </summary>
        /// <param name="id">The ID of the tool</param>
        /// <returns>The tool as a DTO if found</returns>
        Task<ToolDto> GetToolByIdAsync(Guid id);

        /// <summary>
        /// Gets a tool by its name
        /// </summary>
        /// <param name="toolName">The name of the tool</param>
        /// <returns>The tool as a DTO if found, otherwise null</returns>
        Task<ToolDto?> GetToolByNameAsync(string toolName);

        /// <summary>
        /// Gets all tools
        /// </summary>
        /// <returns>List of all tools as DTOs</returns>
        Task<List<ToolDto>> GetAllToolsAsync();

        /// <summary>
        /// Updates a tool
        /// </summary>
        /// <param name="id">The ID of the tool to update</param>
        /// <param name="updateDto">The update data</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> UpdateToolAsync(Guid id, UpdateToolDto updateDto);

        /// <summary>
        /// Deletes a tool by its ID
        /// </summary>
        /// <param name="id">The ID of the tool to delete</param>
        /// <returns>True if successful, otherwise false</returns>
        Task<bool> DeleteToolAsync(Guid id);

        /// <summary>
        /// Checks if a tool with the specified name already exists
        /// </summary>
        /// <param name="toolName">The tool name to check</param>
        /// <returns>True if the tool name exists, otherwise false</returns>
        Task<bool> IsToolNameExistedAsync(string toolName);

        /// <summary>
        /// Searches for tools with pagination
        /// </summary>
        /// <param name="searchTerm">Optional search term for tool name or description</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged result of tools as DTOs</returns>
        Task<PagedResult<ToolDto>> SearchToolsAsync(
            string? searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10);

        /// <summary>
        /// Gets tools with optional filtering by tool name (partial match)
        /// </summary>
        /// <param name="toolName">Optional tool name filter (partial match)</param>
        /// <returns>List of tools as DTOs</returns>
        Task<List<ToolDto>> GetToolsByNameContainingAsync(string? toolName = null);

    }
}

using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.Tools;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class ToolService : IToolService
    {
        private readonly IToolRepository _toolRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ToolService(IUnitOfWork unitOfWork, IToolRepository toolRepository)
        {
            _unitOfWork = unitOfWork;
            _toolRepository = toolRepository;
        }
        /// <summary>
        /// Maps a Tools entity to its DTO representation
        /// </summary>
        private static ToolDto MapToDto(Tools entity)
        {
            return new ToolDto
            {
                Id = entity.Id,
                ToolName = entity.ToolName,
                Description = entity.Description,
                DelFlg = entity.DelFlg,
                CreatedAt = entity.InsDate,
                UpdatedAt = entity.UpdDate
            };
        }

        /// <summary>
        /// Creates a new tool
        /// </summary>
        public async Task<ToolDto> CreateToolAsync(CreateToolDto createDto)
        {
            // Check if tool name already exists
            if (await _toolRepository.IsToolNameExistedAsync(createDto.ToolName))
            {
                throw new InvalidOperationException($"A tool with the name '{createDto.ToolName}' already exists.");
            }

            var tool = new Tools
            {
                ToolName = createDto.ToolName,
                Description = createDto.Description,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _toolRepository.InsertAsync(tool);
            await _unitOfWork.SaveAsync();

            return MapToDto(tool);
        }

        /// <summary>
        /// Gets a tool by its ID
        /// </summary>
        public async Task<ToolDto> GetToolByIdAsync(Guid id)
        {
            var tool = await _toolRepository.GetByIdAsync(id.ToString());

            if (tool == null || tool.DelFlg)
            {
                throw new KeyNotFoundException($"Tool with ID {id} not found.");
            }

            return MapToDto(tool);
        }

        /// <summary>
        /// Gets a tool by its name
        /// </summary>
        public async Task<ToolDto?> GetToolByNameAsync(string toolName)
        {
            var tool = await _toolRepository.GetByToolNameAsync(toolName);

            if (tool == null || tool.DelFlg)
            {
                return null;
            }

            return MapToDto(tool);
        }

        /// <summary>
        /// Gets all tools
        /// </summary>
        public async Task<List<ToolDto>> GetAllToolsAsync()
        {
            var tools = await _toolRepository.GetAllAsync();
            return tools.Where(t => !t.DelFlg).Select(MapToDto).ToList();
        }

        /// <summary>
        /// Updates a tool
        /// </summary>
        public async Task<bool> UpdateToolAsync(Guid id, UpdateToolDto updateDto)
        {
            var tool = await _toolRepository.GetByIdAsync(id.ToString());

            if (tool == null || tool.DelFlg)
            {
                throw new KeyNotFoundException($"Tool with ID {id} not found.");
            }

            // Check if the updated name conflicts with an existing tool (except itself)
            if (updateDto.ToolName != tool.ToolName &&
                await _toolRepository.IsToolNameExistedAsync(updateDto.ToolName))
            {
                throw new InvalidOperationException($"A tool with the name '{updateDto.ToolName}' already exists.");
            }

            tool.ToolName = updateDto.ToolName ?? tool.ToolName;
            tool.Description = updateDto.Description ?? tool.Description;
            tool.UpdDate = DateTime.UtcNow;

            await _toolRepository.ReplaceAsync(id.ToString(), tool);
            await _unitOfWork.SaveAsync();

            return true;
        }

        /// <summary>
        /// Deletes a tool by its ID
        /// </summary>
        public async Task<bool> DeleteToolAsync(Guid id)
        {
            try
            {
                await _toolRepository.DeleteAsync(id.ToString());
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete tool: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Checks if a tool with the specified name already exists
        /// </summary>
        public async Task<bool> IsToolNameExistedAsync(string toolName)
        {
            return await _toolRepository.IsToolNameExistedAsync(toolName);
        }

        /// <summary>
        /// Searches for tools with pagination
        /// </summary>
        public async Task<PagedResult<ToolDto>> SearchToolsAsync(
            string? searchTerm = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _toolRepository.SearchToolsAsync(searchTerm, paginationParams);

            return new PagedResult<ToolDto>
            {
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result.Items.Select(MapToDto)
            };
        }

        /// <summary>
        /// Gets tools with optional filtering by tool name
        /// </summary>
        public async Task<List<ToolDto>> GetToolsByNameContainingAsync(string? toolName = null)
        {
            var tools = await _toolRepository.GetToolsByNameContainingAsync(toolName);
            return tools.Select(MapToDto).ToList();
        }
    }
}

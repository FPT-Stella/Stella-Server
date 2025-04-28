using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToolController : BaseController
    {
        private readonly IToolService _toolService;

        public ToolController(IToolService toolService)
        {
            _toolService = toolService;
        }

        /// <summary>
        /// Creates a new tool
        /// </summary>
        /// <param name="createDto">The tool data to create</param>
        /// <returns>The created tool</returns>
        [HttpPost]
        public async Task<IActionResult> CreateTool([FromBody] CreateToolDto createDto)
        {
            try
            {
                var result = await _toolService.CreateToolAsync(createDto);
                return CreatedAtAction(nameof(GetToolById), new { id = result.Id }, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets a tool by its ID
        /// </summary>
        /// <param name="id">The ID of the tool</param>
        /// <returns>The tool</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetToolById(Guid id)
        {
            try
            {
                var result = await _toolService.GetToolByIdAsync(id);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets a tool by name
        /// </summary>
        /// <param name="name">The name of the tool</param>
        /// <returns>The tool</returns>
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetToolByName(string name)
        {
            try
            {
                var result = await _toolService.GetToolByNameAsync(name);
                if (result == null)
                {
                    return NotFound($"Tool with name '{name}' not found.");
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all tools
        /// </summary>
        /// <returns>List of all tools</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllTools()
        {
            try
            {
                var results = await _toolService.GetAllToolsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Updates a tool
        /// </summary>
        /// <param name="id">The ID of the tool to update</param>
        /// <param name="updateDto">The update data</param>
        /// <returns>NoContent if successful</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTool(Guid id, [FromBody] UpdateToolDto updateDto)
        {
            try
            {
                var success = await _toolService.UpdateToolAsync(id, updateDto);
                return success ? NoContent() : BadRequest();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a tool
        /// </summary>
        /// <param name="id">The ID of the tool to delete</param>
        /// <returns>NoContent if successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTool(Guid id)
        {
            try
            {
                var success = await _toolService.DeleteToolAsync(id);
                return success ? NoContent() : BadRequest();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Checks if a tool name already exists
        /// </summary>
        /// <param name="toolName">The tool name to check</param>
        /// <returns>True if exists, otherwise false</returns>
        [HttpGet("check-name")]
        public async Task<IActionResult> CheckToolNameExists([FromQuery] string toolName)
        {
            try
            {
                var exists = await _toolService.IsToolNameExistedAsync(toolName);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches for tools with pagination
        /// </summary>
        /// <param name="searchTerm">Search term for tool name or description (optional)</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged result of tools</returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchTools(
            [FromQuery] string? searchTerm = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var results = await _toolService.SearchToolsAsync(
                    searchTerm,
                    pageNumber,
                    pageSize);

                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets tools with optional filtering by tool name
        /// </summary>
        /// <param name="toolName">Optional tool name filter (partial match)</param>
        /// <returns>List of tools matching the criteria</returns>
        [HttpGet("filter")]
        public async Task<IActionResult> GetToolsByNameContaining([FromQuery] string? toolName = null)
        {
            try
            {
                var results = await _toolService.GetToolsByNameContainingAsync(toolName);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

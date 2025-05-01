using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.SubjectTools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectToolController : BaseController
    {
        private readonly ISubjectToolService _subjectToolService;

        public SubjectToolController(ISubjectToolService subjectToolService)
        {
            _subjectToolService = subjectToolService;
        }

        /// <summary>
        /// Creates a new mapping between a Subject and a Tool
        /// </summary>
        /// <param name="createMappingDto">The data for the new mapping</param>
        /// <returns>Action result indicating success or failure</returns>
        [HttpPost]
        public async Task<IActionResult> CreateMapping([FromBody] CreateSubjectToolDto createMappingDto)
        {
            try
            {
                await _subjectToolService.CreateMappingAsync(createMappingDto);
                return CreatedAtAction(nameof(CheckMappingExists),
                    new { subjectId = createMappingDto.SubjectId, toolId = createMappingDto.ToolId },
                    createMappingDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Creates multiple mappings between Subjects and Tools in a batch operation
        /// </summary>
        /// <param name="createMappingBatchDto">The batch of mappings to create</param>
        /// <returns>The successfully created mappings</returns>
        [HttpPost("batch")]
        public async Task<IActionResult> CreateMappingBatch([FromBody] CreateSubjectToolBatchDto createMappingBatchDto)
        {
            try
            {
                var result = await _subjectToolService.CreateMappingBatchAsync(createMappingBatchDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Updates multiple mappings between Subjects and Tools in a batch operation
        /// </summary>
        /// <param name="updateMappingBatchDto">The batch of mappings to update</param>
        /// <returns>The result of the update operation</returns>
        [HttpPut("batch")]
        public async Task<IActionResult> UpdateMappingBatch([FromBody] UpdateSubjectToolBatchDto updateMappingBatchDto)
        {
            try
            {
                var result = await _subjectToolService.UpdateMappingsAsync(updateMappingBatchDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Updates Subject-Tool mappings by replacing all Tools for a given Subject
        /// </summary>
        /// <param name="dto">The patch DTO containing Subject ID and new Tool IDs</param>
        /// <returns>Action result indicating success or failure</returns>
        [HttpPatch("subject-tools")]
        public async Task<IActionResult> UpdateSubjectToolMapping([FromBody] PatchSubjectToolMappingDto dto)
        {
            try
            {
                await _subjectToolService.UpdateSubjectToolMappingAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Updates Tool-Subject mappings by replacing all Subjects for a given Tool.
        /// This is the reverse operation of UpdateSubjectToolMapping.
        /// </summary>
        /// <param name="dto">The patch DTO containing Tool ID and new Subject IDs</param>
        /// <returns>Action result indicating success or failure</returns>
        [HttpPatch("tool-subjects")]
        public async Task<IActionResult> UpdateToolSubjectMapping([FromBody] PatchToolSubjectMappingDto dto)
        {
            try
            {
                await _subjectToolService.UpdateToolSubjectMappingAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all Tool IDs associated with a specific Subject
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <returns>List of Tool IDs</returns>
        [HttpGet("subject/{subjectId}/tools")]
        public async Task<IActionResult> GetToolsBySubjectId(Guid subjectId)
        {
            try
            {
                var toolIds = await _subjectToolService.GetToolIdsBySubjectIdAsync(subjectId);
                return Ok(toolIds);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all Subject IDs associated with a specific Tool
        /// </summary>
        /// <param name="toolId">The Tool ID</param>
        /// <returns>List of Subject IDs</returns>
        [HttpGet("tool/{toolId}/subjects")]
        public async Task<IActionResult> GetSubjectsByToolId(Guid toolId)
        {
            try
            {
                var subjectIds = await _subjectToolService.GetSubjectIdsByToolIdAsync(toolId);
                return Ok(subjectIds);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all Tools with names associated with a specific Subject
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <returns>List of Tools with names</returns>
        [HttpGet("subject/{subjectId}/tools-with-names")]
        public async Task<IActionResult> GetToolsWithNameBySubjectId(Guid subjectId)
        {
            try
            {
                var toolsWithName = await _subjectToolService.GetToolsWithNameBySubjectIdAsync(subjectId);
                return Ok(toolsWithName);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Checks if a mapping between a specific Subject and Tool exists
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <param name="toolId">The Tool ID</param>
        /// <returns>Boolean indicating if the mapping exists</returns>
        [HttpGet("exists")]
        public async Task<IActionResult> CheckMappingExists([FromQuery] Guid subjectId, [FromQuery] Guid toolId)
        {
            try
            {
                var exists = await _subjectToolService.IsMappingExistedAsync(subjectId, toolId);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes all mappings associated with a specific Subject
        /// </summary>
        /// <param name="subjectId">The Subject ID</param>
        /// <returns>Action result indicating success or failure</returns>
        [HttpDelete("subject/{subjectId}")]
        public async Task<IActionResult> DeleteMappingsBySubjectId(Guid subjectId)
        {
            try
            {
                await _subjectToolService.DeleteMappingsBySubjectIdAsync(subjectId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes all mappings associated with a specific Tool
        /// </summary>
        /// <param name="toolId">The Tool ID</param>
        /// <returns>Action result indicating success or failure</returns>
        [HttpDelete("tool/{toolId}")]
        public async Task<IActionResult> DeleteMappingsByToolId(Guid toolId)
        {
            try
            {
                await _subjectToolService.DeleteMappingsByToolIdAsync(toolId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
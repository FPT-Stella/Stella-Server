using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.SubjectComboSubjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectComboSubjectController : BaseController
    {
        private readonly ISubjectComboSubjectService _subjectComboSubjectService;

        public SubjectComboSubjectController(ISubjectComboSubjectService subjectComboSubjectService)
        {
            _subjectComboSubjectService = subjectComboSubjectService;
        }

        /// <summary>
        /// Creates a new mapping between a subject combo and a subject
        /// </summary>
        /// <param name="createDto">The mapping data to create</param>
        /// <returns>The created mapping</returns>
        [HttpPost]
        public async Task<IActionResult> CreateMapping([FromBody] CreateSubjectComboSubjectDto createDto)
        {
            try
            {
                var result = await _subjectComboSubjectService.CreateMappingAsync(createDto);
                return CreatedAtAction(nameof(GetMappingById), new { id = result.Id }, result);
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
        /// Creates multiple mappings between subject combos and subjects in a batch operation
        /// </summary>
        /// <param name="batchDto">The batch of mappings to create</param>
        /// <returns>The successfully created mappings</returns>
        [HttpPost("batch")]
        public async Task<IActionResult> CreateMappingBatch([FromBody] CreateSubjectComboSubjectBatchDto batchDto)
        {
            try
            {
                var result = await _subjectComboSubjectService.CreateMappingBatchAsync(batchDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets a mapping by its ID
        /// </summary>
        /// <param name="id">The ID of the mapping</param>
        /// <returns>The mapping if found</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMappingById(Guid id)
        {
            try
            {
                var result = await _subjectComboSubjectService.GetMappingByIdAsync(id);
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
        /// Gets all mappings for a specific subject combo
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        /// <returns>List of mappings</returns>
        [HttpGet("combo/{subjectComboId}")]
        public async Task<IActionResult> GetMappingsBySubjectComboId(Guid subjectComboId)
        {
            try
            {
                var results = await _subjectComboSubjectService.GetMappingsBySubjectComboIdAsync(subjectComboId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all mappings for a specific subject
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of mappings</returns>
        [HttpGet("subject/{subjectId}")]
        public async Task<IActionResult> GetMappingsBySubjectId(Guid subjectId)
        {
            try
            {
                var results = await _subjectComboSubjectService.GetMappingsBySubjectIdAsync(subjectId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all mappings
        /// </summary>
        /// <returns>List of all mappings</returns>
        [HttpGet]
        public async Task<IActionResult> GetAllMappings()
        {
            try
            {
                var results = await _subjectComboSubjectService.GetAllMappingsAsync();
                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes a mapping by its ID
        /// </summary>
        /// <param name="id">The ID of the mapping to delete</param>
        /// <returns>NoContent if successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMapping(Guid id)
        {
            try
            {
                var success = await _subjectComboSubjectService.DeleteMappingAsync(id);
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
        /// Deletes all mappings for a specific subject combo
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        /// <returns>NoContent if successful</returns>
        [HttpDelete("combo/{subjectComboId}")]
        public async Task<IActionResult> DeleteMappingsBySubjectComboId(Guid subjectComboId)
        {
            try
            {
                var success = await _subjectComboSubjectService.DeleteMappingsBySubjectComboIdAsync(subjectComboId);
                return success ? NoContent() : BadRequest();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes all mappings for a specific subject
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>NoContent if successful</returns>
        [HttpDelete("subject/{subjectId}")]
        public async Task<IActionResult> DeleteMappingsBySubjectId(Guid subjectId)
        {
            try
            {
                var success = await _subjectComboSubjectService.DeleteMappingsBySubjectIdAsync(subjectId);
                return success ? NoContent() : BadRequest();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all subject IDs associated with a specific subject combo
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        /// <returns>List of subject IDs</returns>
        [HttpGet("combo/{subjectComboId}/subjects")]
        public async Task<IActionResult> GetSubjectIdsBySubjectComboId(Guid subjectComboId)
        {
            try
            {
                var results = await _subjectComboSubjectService.GetSubjectIdsBySubjectComboIdAsync(subjectComboId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all subject combo IDs that contain a specific subject
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of subject combo IDs</returns>
        [HttpGet("subject/{subjectId}/combos")]
        public async Task<IActionResult> GetSubjectComboIdsBySubjectId(Guid subjectId)
        {
            try
            {
                var results = await _subjectComboSubjectService.GetSubjectComboIdsBySubjectIdAsync(subjectId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Checks if a mapping between a subject combo and a subject already exists
        /// </summary>
        /// <param name="subjectComboId">The subject combo ID</param>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>True if exists, otherwise false</returns>
        [HttpGet("check")]
        public async Task<IActionResult> CheckMappingExists(
            [FromQuery] Guid subjectComboId,
            [FromQuery] Guid subjectId)
        {
            try
            {
                var exists = await _subjectComboSubjectService.IsMappingExistedAsync(subjectComboId, subjectId);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Searches for subject combo subject mappings with pagination
        /// </summary>
        /// <param name="subjectComboId">Optional subject combo ID filter</param>
        /// <param name="subjectId">Optional subject ID filter</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged result of mappings</returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchMappings(
            [FromQuery] Guid? subjectComboId = null,
            [FromQuery] Guid? subjectId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var results = await _subjectComboSubjectService.SearchMappingsAsync(
                    subjectComboId,
                    subjectId,
                    pageNumber,
                    pageSize);

                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpPut("batch")]
        public async Task<IActionResult> UpdateMappingsBatch([FromBody] UpdateSubjectComboSubjectBatchDto batchDto)
        {
            try
            {
                var result = await _subjectComboSubjectService.UpdateMappingsBatchAsync(batchDto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Updates all subject mappings for a specific subject combo
        /// </summary>
        /// <param name="dto">The patch DTO containing subject combo ID and new subject IDs</param>
        [HttpPatch("subject-combo-mapping")]
        public async Task<IActionResult> UpdateSubjectComboMapping([FromBody] PatchSubjectComboMappingDto dto)
        {
            try
            {
                await _subjectComboSubjectService.UpdateSubjectComboMappingAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        /// <summary>
        /// Updates all subject combo mappings for a specific subject
        /// </summary>
        /// <param name="dto">The patch DTO containing subject ID and new subject combo IDs</param>
        [HttpPatch("subject-mapping")]
        public async Task<IActionResult> UpdateSubjectMapping([FromBody] PatchSubjectMappingDto dto)
        {
            try
            {
                await _subjectComboSubjectService.UpdateSubjectMappingAsync(dto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.SubjectCombos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectComboController : BaseController
    {
        private readonly ISubjectComboService _subjectComboService;
        public SubjectComboController(ISubjectComboService subjectComboService)
        {
            _subjectComboService = subjectComboService;
        }
        /// <summary>
        /// Creates a new subject combo
        /// </summary>
        /// <param name="createDto">The subject combo data to create</param>
        /// <returns>The created subject combo</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSubjectComboDto createDto)
        {
            try
            {
                var result = await _subjectComboService.CreateComboAsync(createDto);
                return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
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
        /// Searches for subject combos with pagination
        /// </summary>
        /// <param name="searchTerm">Search term (optional)</param>
        /// <param name="programId">Program ID filter (optional)</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged result of subject combos</returns>
        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? searchTerm = null,
            [FromQuery] Guid? programId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var results = await _subjectComboService.SearchCombosAsync(
                    searchTerm ?? string.Empty,
                    programId,
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
        /// Gets a subject combo by its ID
        /// </summary>
        /// <param name="id">The ID of the subject combo</param>
        /// <returns>The subject combo</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            try
            {
                var result = await _subjectComboService.GetComboByIdAsync(id);
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
        /// Gets a subject combo by name
        /// </summary>
        /// <param name="name">The name of the subject combo</param>
        /// <returns>The subject combo</returns>
        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetByName(string name)
        {
            try
            {
                var result = await _subjectComboService.GetComboByNameAsync(name);
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
        /// Gets all subject combos for a specific program
        /// </summary>
        /// <param name="programId">The program ID</param>
        /// <returns>List of subject combos</returns>
        [HttpGet("program/{programId}")]
        public async Task<IActionResult> GetByProgramId(Guid programId)
        {
            try
            {
                var results = await _subjectComboService.GetCombosByProgramIdAsync(programId);
                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Updates a subject combo
        /// </summary>
        /// <param name="id">The ID of the subject combo to update</param>
        /// <param name="updateDto">The update data</param>
        /// <returns>NoContent if successful</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateSubjectComboDto updateDto)
        {
            try
            {
                var success = await _subjectComboService.UpdateComboAsync(id, updateDto);
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
        /// Deletes a subject combo
        /// </summary>
        /// <param name="id">The ID of the subject combo to delete</param>
        /// <returns>NoContent if successful</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            try
            {
                var success = await _subjectComboService.DeleteComboAsync(id);
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
        /// Deletes all subject combos for a specific program
        /// </summary>
        /// <param name="programId">The program ID</param>
        /// <returns>NoContent if successful</returns>
        [HttpDelete("program/{programId}")]
        public async Task<IActionResult> DeleteByProgramId(Guid programId)
        {
            try
            {
                var success = await _subjectComboService.DeleteCombosByProgramIdAsync(programId);
                return success ? NoContent() : BadRequest();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Checks if a combo name already exists within a program
        /// </summary>
        /// <param name="programId">The program ID</param>
        /// <param name="comboName">The combo name to check</param>
        /// <returns>True if exists, otherwise false</returns>
        [HttpGet("check-name")]
        public async Task<IActionResult> CheckComboNameExists(
            [FromQuery] Guid programId,
            [FromQuery] string comboName)
        {
            try
            {
                var exists = await _subjectComboService.IsComboNameExistedAsync(programId, comboName);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

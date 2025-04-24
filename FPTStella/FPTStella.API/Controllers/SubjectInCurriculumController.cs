using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.SubjectInCurriculums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectInCurriculumController : BaseController
    {
        private readonly ISubjectInCurriculumService _subjectInCurriculumService;

        public SubjectInCurriculumController(ISubjectInCurriculumService subjectInCurriculumService)
        {
            _subjectInCurriculumService = subjectInCurriculumService ?? throw new ArgumentNullException(nameof(subjectInCurriculumService));
        }

        /// <summary>
        /// Creates a new mapping between a subject and a curriculum.
        /// </summary>
        /// <param name="createDto">The create mapping data</param>
        /// <returns>The newly created mapping</returns>
        [HttpPost]
        [ProducesResponseType(typeof(SubjectInCurriculumDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateMapping([FromBody] CreateSubjectInCurriculumDto createDto)
        {
            try
            {
                var result = await _subjectInCurriculumService.CreateMappingAsync(createDto);
                return CreatedAtAction(nameof(GetMappingById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets a specific mapping by its ID.
        /// </summary>
        /// <param name="id">The mapping ID</param>
        /// <returns>The mapping with the specified ID</returns>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(SubjectInCurriculumDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMappingById(Guid id)
        {
            try
            {
                var mapping = await _subjectInCurriculumService.GetMappingByIdAsync(id);
                return Ok(mapping);
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
        /// Gets all mappings.
        /// </summary>
        /// <returns>A list of all active mappings</returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<SubjectInCurriculumDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllMappings()
        {
            try
            {
                var mappings = await _subjectInCurriculumService.GetAllMappingsAsync();
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Updates an existing mapping.
        /// </summary>
        /// <param name="id">The mapping ID</param>
        /// <param name="updateDto">The mapping update data</param>
        /// <returns>No content if successful</returns>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateMapping(Guid id, [FromBody] UpdateSubjectInCurriculumDto updateDto)
        {
            try
            {
                await _subjectInCurriculumService.UpdateMappingAsync(id, updateDto);
                return NoContent();
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
        /// Deletes a mapping by ID.
        /// </summary>
        /// <param name="id">The mapping ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMapping(Guid id)
        {
            try
            {
                await _subjectInCurriculumService.DeleteMappingAsync(id);
                return NoContent();
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
        /// Gets all mappings for a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>List of mappings for the curriculum</returns>
        [HttpGet("curriculum/{curriculumId:guid}")]
        [ProducesResponseType(typeof(List<SubjectInCurriculumDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMappingsByCurriculumId(Guid curriculumId)
        {
            try
            {
                var mappings = await _subjectInCurriculumService.GetMappingsByCurriculumIdAsync(curriculumId);
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets all mappings for a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of mappings for the subject</returns>
        [HttpGet("subject/{subjectId:guid}")]
        [ProducesResponseType(typeof(List<SubjectInCurriculumDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMappingsBySubjectId(Guid subjectId)
        {
            try
            {
                var mappings = await _subjectInCurriculumService.GetMappingsBySubjectIdAsync(subjectId);
                return Ok(mappings);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets subject IDs associated with a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>List of subject IDs</returns>
        [HttpGet("curriculum/{curriculumId:guid}/subjects")]
        [ProducesResponseType(typeof(List<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSubjectIdsByCurriculumId(Guid curriculumId)
        {
            try
            {
                var subjectIds = await _subjectInCurriculumService.GetSubjectIdsByCurriculumIdAsync(curriculumId);
                return Ok(subjectIds);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Gets curriculum IDs associated with a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of curriculum IDs</returns>
        [HttpGet("subject/{subjectId:guid}/curricula")]
        [ProducesResponseType(typeof(List<Guid>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurriculumIdsBySubjectId(Guid subjectId)
        {
            try
            {
                var curriculumIds = await _subjectInCurriculumService.GetCurriculumIdsBySubjectIdAsync(subjectId);
                return Ok(curriculumIds);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes all mappings for a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("subject/{subjectId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMappingsBySubjectId(Guid subjectId)
        {
            try
            {
                await _subjectInCurriculumService.DeleteMappingsBySubjectIdAsync(subjectId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Deletes all mappings for a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>No content if successful</returns>
        [HttpDelete("curriculum/{curriculumId:guid}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteMappingsByCurriculumId(Guid curriculumId)
        {
            try
            {
                await _subjectInCurriculumService.DeleteMappingsByCurriculumIdAsync(curriculumId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Checks if a mapping exists between a subject and curriculum.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>True if mapping exists, otherwise false</returns>
        [HttpGet("exists")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DoesMappingExist([FromQuery] Guid subjectId, [FromQuery] Guid curriculumId)
        {
            try
            {
                var exists = await _subjectInCurriculumService.IsMappingExistedAsync(subjectId, curriculumId);
                return Ok(exists);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

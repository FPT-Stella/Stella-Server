using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.Materials;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialController : BaseController
    {
        private readonly IMaterialService _materialService;
        public MaterialController(IMaterialService materialService)
        {
            _materialService = materialService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMaterials()
        {
            try
            {
                var materials = await _materialService.GetAllMaterialsAsync();
                return Ok(materials);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetMaterialById(string id)
        {
            try
            {
                var material = await _materialService.GetMaterialByIdAsync(id);
                return Ok(material);
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

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetMaterialByName(string name)
        {
            try
            {
                var material = await _materialService.GetMaterialByNameAsync(name);
                if (material == null)
                {
                    return NotFound($"Material with name '{name}' not found.");
                }
                return Ok(material);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("subject/{subjectId}")]
        public async Task<IActionResult> GetMaterialsBySubjectId(Guid subjectId)
        {
            try
            {
                var materials = await _materialService.GetMaterialsBySubjectIdAsync(subjectId);
                return Ok(materials);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("type/{type}")]
        public async Task<IActionResult> GetMaterialsByType(string type)
        {
            try
            {
                var materials = await _materialService.GetMaterialsByTypeAsync(type);
                return Ok(materials);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Search for materials with filters and pagination
        /// </summary>
        /// <param name="searchTerm">Optional search term</param>
        /// <param name="subjectId">Optional subject ID filter</param>
        /// <param name="materialType">Optional material type filter</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged results of materials</returns>
        [HttpGet("search")]
        public async Task<IActionResult> SearchMaterials(
            [FromQuery] string? searchTerm = null,
            [FromQuery] Guid? subjectId = null,
            [FromQuery] string? materialType = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var results = await _materialService.SearchMaterialsAsync(
                    searchTerm,
                    subjectId,
                    materialType,
                    pageNumber,
                    pageSize);

                return Ok(results);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateMaterial([FromBody] CreateMaterialDto createMaterialDto)
        {
            try
            {
                var material = await _materialService.CreateMaterialAsync(createMaterialDto);
                return CreatedAtAction(nameof(GetMaterialById), new { id = material.Id }, material);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(string id, [FromBody] UpdateMaterialDto updateMaterialDto)
        {
            try
            {
                var success = await _materialService.UpdateMaterialAsync(id, updateMaterialDto);
                return success ? NoContent() : BadRequest("Failed to update material.");
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

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(string id)
        {
            try
            {
                var success = await _materialService.DeleteMaterialAsync(id);
                return success ? NoContent() : BadRequest("Failed to delete material.");
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

        [HttpDelete("subject/{subjectId}")]
        public async Task<IActionResult> DeleteMaterialsBySubjectId(Guid subjectId)
        {
            try
            {
                var success = await _materialService.DeleteMaterialsBySubjectIdAsync(subjectId);
                return success ? NoContent() : BadRequest("Failed to delete materials for subject.");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.PLOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PLOController : BaseController
    {
        private readonly IPLOService _ploService;

        public PLOController(IPLOService ploService)
        {
            _ploService = ploService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePLO([FromBody] CreatePLOsDto createPLOsDto)
        {
            try
            {
                var plo = await _ploService.CreatePLOAsync(createPLOsDto);
                return CreatedAtAction(nameof(GetPLOById), new { id = plo.Id }, plo);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPLOs()
        {
            try
            {
                var plos = await _ploService.GetAllPLOsAsync();
                return Ok(plos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPLOById(Guid id)
        {
            try
            {
                var plo = await _ploService.GetPLOByIdAsync(id);
                return Ok(plo);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("curriculum/{curriculumId}")]
        public async Task<IActionResult> GetPLOsByCurriculumId(Guid curriculumId)
        {
            try
            {
                var plos = await _ploService.GetPLOsByCurriculumIdAsync(curriculumId);
                return Ok(plos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("curriculums")]
        public async Task<IActionResult> GetPLOsByCurriculumIds([FromQuery] List<Guid> curriculumIds)
        {
            try
            {
                var plos = await _ploService.GetPLOsByCurriculumIdsAsync(curriculumIds);
                return Ok(plos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePLO(Guid id, [FromBody] UpdatePLOsDto updatePLOsDto)
        {
            try
            {
                await _ploService.UpdatePLOAsync(id, updatePLOsDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePLO(Guid id)
        {
            try
            {
                await _ploService.DeletePLOAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("curriculum/{curriculumId}")]
        public async Task<IActionResult> DeletePLOsByCurriculumId(Guid curriculumId)
        {
            try
            {
                await _ploService.DeletePLOsByCurriculumIdAsync(curriculumId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

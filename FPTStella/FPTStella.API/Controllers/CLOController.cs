using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.CLOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CLOController : BaseController
    {
        private readonly ICLOService _cloService;

        public CLOController(ICLOService cloService)
        {
            _cloService = cloService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCLO([FromBody] CreateCLODto createCloDto)
        {
            try
            {
                var cloId = await _cloService.CreateCloAsync(createCloDto);
                return Ok(new { Id = cloId });
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("subject/{subjectId}")]
        public async Task<IActionResult> GetCLOsBySubjectId(Guid subjectId)
        {
            try
            {
                var clos = await _cloService.GetClosBySubjectIdAsync(subjectId);
                return Ok(clos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("subject/{subjectId}")]
        public async Task<IActionResult> DeleteCLOsBySubjectId(Guid subjectId)
        {
            try
            {
                await _cloService.DeleteClosBySubjectIdAsync(subjectId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
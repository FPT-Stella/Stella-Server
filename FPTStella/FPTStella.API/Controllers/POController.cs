using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.POs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class POController : BaseController
    {
        private readonly IPOService _poService;

        public POController(IPOService poService)
        {
            _poService = poService;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePO([FromBody] CreatePOsDto createPOsDto)
        {
            try
            {
                var po = await _poService.CreatePOAsync(createPOsDto);
                return CreatedAtAction(nameof(GetPOById), new { id = po.Id }, po);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPOs()
        {
            try
            {
                var pos = await _poService.GetAllPOsAsync();
                return Ok(pos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPOById(Guid id)
        {
            try
            {
                var po = await _poService.GetPOByIdAsync(id);
                return Ok(po);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("program/{programId}")]
        public async Task<IActionResult> GetPOsByProgramId(Guid programId)
        {
            try
            {
                var pos = await _poService.GetPOsByProgramIdAsync(programId);
                return Ok(pos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("programs")]
        public async Task<IActionResult> GetPOsByProgramIds([FromQuery] List<Guid> programIds)
        {
            try
            {
                var pos = await _poService.GetPOsByProgramIdsAsync(programIds);
                return Ok(pos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePO(Guid id, [FromBody] UpdatePOsDto updatePOsDto)
        {
            try
            {
                await _poService.UpdatePOAsync(id, updatePOsDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePO(Guid id)
        {
            try
            {
                await _poService.DeletePOAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("program/{programId}")]
        public async Task<IActionResult> DeletePOsByProgramId(Guid programId)
        {
            try
            {
                await _poService.DeletePOsByProgramIdAsync(programId);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

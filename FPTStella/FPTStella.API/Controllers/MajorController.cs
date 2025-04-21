using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.Majors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorController : BaseController
    {
        private readonly IMajorService _majorService;

        public MajorController(IMajorService majorService)
        {
            _majorService = majorService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateMajor([FromBody] CreateMajorDto createMajorDto)
        {
            try
            {
                var result = await _majorService.CreateMajorAsync(createMajorDto);
                return CreatedAtAction(nameof(GetMajorById), new { id = result.Id }, result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetAllMajors()
        {
            try
            {
                var result = await _majorService.GetAllMajorsAsync();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetMajorById(Guid id)
        {
            try
            {
                var result = await _majorService.GetMajorByIdAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateMajor(Guid id, [FromBody] UpdateMajorDto updateMajorDto)
        {
            try
            {
                await _majorService.UpdateMajorAsync(id, updateMajorDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteMajor(Guid id)
        {
            try
            {
                await _majorService.DeleteMajorAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpGet("exists")]
        public async Task<IActionResult> MajorExists([FromQuery] string majorName)
        {
            try
            {
                var exists = await _majorService.MajorExistsAsync(majorName);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpGet("exists/{id:guid}")]
        public async Task<IActionResult> MajorExistsById(Guid id)
        {
            try
            {
                var exists = await _majorService.MajorExistsByIdAsync(id);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

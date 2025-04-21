using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.Programs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProgramController : BaseController
    {
        private readonly IProgramService _programService;

        public ProgramController(IProgramService programService)
        {
            _programService = programService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateProgram([FromBody] CreateProgramDto createProgramDto)
        {
            try
            {
                var programDto = await _programService.CreateProgramAsync(createProgramDto);
                return CreatedAtAction(nameof(GetProgramById), new { id = programDto.Id }, programDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProgramById(string id)
        {
            try
            {
                var programDto = await _programService.GetProgramByIdAsync(id);
                return Ok(programDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("program-code/{programCode}")]
        public async Task<IActionResult> GetProgramByProgramCode(string programCode)
        {
            try
            {
                var programDto = await _programService.GetProgramByProgramCodeAsync(programCode);
                return Ok(programDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("major/{majorId}")]
        public async Task<IActionResult> GetProgramByMajorId(Guid majorId)
        {
            try
            {
                var programDto = await _programService.GetProgramByMajorIdAsync(majorId);
                return Ok(programDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("program-name/{programName}")]
        public async Task<IActionResult> GetProgramByProgramName(string programName)
        {
            try
            {
                var programDto = await _programService.GetProgramByProgramNameAsync(programName);
                return Ok(programDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("major/{majorId}/program-name/{programName}")]
        public async Task<IActionResult> GetProgramByMajorIdAndProgramName(Guid majorId, string programName)
        {
            try
            {
                var programDto = await _programService.GetProgramByMajorIdAndProgramNameAsync(majorId, programName);
                return Ok(programDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("major/{majorId}/program-code/{programCode}")]
        public async Task<IActionResult> GetProgramByMajorIdAndProgramCode(Guid majorId, string programCode)
        {
            try
            {
                var programDto = await _programService.GetProgramByMajorIdAndProgramCodeAsync(majorId, programCode);
                return Ok(programDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProgram(string id, [FromBody] UpdateProgramDto updateProgramDto)
        {
            try
            {
                await _programService.UpdateProgramAsync(id, updateProgramDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProgram(string id)
        {
            try
            {
                await _programService.DeleteProgramAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
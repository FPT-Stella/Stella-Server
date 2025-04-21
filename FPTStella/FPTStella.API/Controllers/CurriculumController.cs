using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.Curriculums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurriculumController : BaseController
    {
        private readonly ICurriculumService _curriculumService;

        public CurriculumController(ICurriculumService curriculumService)
        {
            _curriculumService = curriculumService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCurriculum([FromBody] CreateCurriculumDto createCurriculumDto)
        {
            try
            {
                var curriculumDto = await _curriculumService.CreateCurriculumAsync(createCurriculumDto);
                return CreatedAtAction(nameof(GetCurriculumById), new { id = curriculumDto.Id }, curriculumDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCurriculums()
        {
            try
            {
                var curriculums = await _curriculumService.GetAllCurriculumsAsync();
                return Ok(curriculums);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetCurriculumById(Guid id)
        {
            try
            {
                var curriculumDto = await _curriculumService.GetCurriculumByIdAsync(id);
                return Ok(curriculumDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("curriculum-code/{curriculumCode}")]
        public async Task<IActionResult> GetCurriculumByCurriculumCode(string curriculumCode)
        {
            try
            {
                var curriculumDto = await _curriculumService.GetCurriculumByCurriculumCodeAsync(curriculumCode);
                return Ok(curriculumDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("curriculum-name/{curriculumName}")]
        public async Task<IActionResult> GetCurriculumByCurriculumName(string curriculumName)
        {
            try
            {
                var curriculumDto = await _curriculumService.GetCurriculumByCurriculumNameAsync(curriculumName);
                return Ok(curriculumDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("program/{programId:guid}")]
        public async Task<IActionResult> GetCurriculumsByProgramId(Guid programId)
        {
            try
            {
                var curriculums = await _curriculumService.GetCurriculumsByProgramIdAsync(programId);
                return Ok(curriculums);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCurriculum(Guid id, [FromBody] UpdateCurriculumDto updateCurriculumDto)
        {
            try
            {
                await _curriculumService.UpdateCurriculumAsync(id, updateCurriculumDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCurriculum(Guid id)
        {
            try
            {
                await _curriculumService.DeleteCurriculumAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("exists/curriculum-code/{curriculumCode}")]
        public async Task<IActionResult> IsCurriculumCodeExisted(string curriculumCode)
        {
            try
            {
                var exists = await _curriculumService.IsCurriculumCodeExistedAsync(curriculumCode);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("exists/curriculum-name/{curriculumName}")]
        public async Task<IActionResult> IsCurriculumNameExisted(string curriculumName)
        {
            try
            {
                var exists = await _curriculumService.IsCurriculumNameExistedAsync(curriculumName);
                return Ok(new { exists });
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

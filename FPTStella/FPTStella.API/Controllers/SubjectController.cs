using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.Subjects;
using FPTStella.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : BaseController
    {
        private readonly ISubjectService _subjectService;

        public SubjectController(ISubjectService subjectService)
        {
            _subjectService = subjectService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Subjects>>> GetAllSubjects()
        {
            var result = await _subjectService.GetAllSubjectsAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Subjects>> GetById(string id)
        {
            try
            {
                var subject = await _subjectService.GetSubjectByIdAsync(id);
                return Ok(subject);
            }
            catch (KeyNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        [HttpGet("degreeLevel/{degreeLevel}")]
        public async Task<ActionResult<List<Subjects>>> GetByDegreeLevel(string degreeLevel)
        {
            var result = await _subjectService.GetSubjectsByDegreeLevelAsync(degreeLevel);
            return Ok(result);
        }

        [HttpGet("subjectCode/{code}")]
        public async Task<ActionResult<Subjects?>> GetBySubjectCode(string code)
        {
            var result = await _subjectService.GetBySubjectCodeAsync(code);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpGet("subjectName/{name}")]
        public async Task<ActionResult<Subjects?>> GetBySubjectName(string name)
        {
            var result = await _subjectService.GetBySubjectNameAsync(name);
            return result != null ? Ok(result) : NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateSubjectDto dto)
        {
            try
            {
                await _subjectService.CreateSubjectAsync(dto);
                return CreatedAtAction(nameof(GetBySubjectCode), new { code = dto.SubjectCode }, dto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(string id, [FromBody] UpdateSubjectDto dto)
        {
            try
            {
                var success = await _subjectService.UpdateSubjectAsync(id, dto);
                return success ? NoContent() : BadRequest();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                var success = await _subjectService.DeleteSubjectAsync(id);
                return success ? NoContent() : BadRequest();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        // GET: api/subjects/code-name?code=SUB123&name=Math
        [HttpGet("code-name")]
        public async Task<IActionResult> GetBySubjectCodeAndSubjectName([FromQuery] string code, [FromQuery] string name)
        {
            var result = await _subjectService.GetBySubjectCodeAndSubjectNameAsync(code, name);
            return result == null ? NotFound("Subject not found.") : Ok(result);
        }

        // GET: api/subjects/major-code?majorId=GUID_HERE&code=SUB123
        [HttpGet("major-code")]
        public async Task<IActionResult> GetByMajorIdAndSubjectCode([FromQuery] Guid majorId, [FromQuery] string code)
        {
            var result = await _subjectService.GetByMajorIdAndSubjectCodeAsync(majorId, code);
            return result == null ? NotFound("Subject not found.") : Ok(result);
        }

        // GET: api/subjects/major-name?majorId=GUID_HERE&name=Math
        [HttpGet("major-name")]
        public async Task<IActionResult> GetByMajorIdAndSubjectName([FromQuery] Guid majorId, [FromQuery] string name)
        {
            var result = await _subjectService.GetByMajorIdAndSubjectNameAsync(majorId, name);
            return result == null ? NotFound("Subject not found.") : Ok(result);
        }

    }
}

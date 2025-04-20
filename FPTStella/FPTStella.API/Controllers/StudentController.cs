using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.Students;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : BaseController
    {
        private readonly IStudentService _studentService;

        public StudentController(IStudentService studentService)
        {
            _studentService = studentService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto)
        {
            try
            {
                var studentDto = await _studentService.CreateStudentAsync(createStudentDto);
                return CreatedAtAction(nameof(GetStudentById), new { id = studentDto.Id }, studentDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudentById(string id)
        {
            try
            {
                var studentDto = await _studentService.GetStudentByIdAsync(id);
                return Ok(studentDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("student-code/{studentCode}")]
        public async Task<IActionResult> GetStudentByStudentCode(string studentCode)
        {
            try
            {
                var studentDto = await _studentService.GetStudentByStudentCodeAsync(studentCode);
                return Ok(studentDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetStudentByUserId(string userId)
        {
            try
            {
                var studentDto = await _studentService.GetStudentByUserIdAsync(userId);
                return Ok(studentDto);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStudent(string id, [FromBody] UpdateStudentDto updateStudentDto)
        {
            try
            {
                await _studentService.UpdateStudentAsync(id, updateStudentDto);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(string id)
        {
            try
            {
                await _studentService.DeleteStudentAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

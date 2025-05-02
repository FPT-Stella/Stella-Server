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
        [HttpGet("all")]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var studentDtos = await _studentService.GetAllStudentsAsync();
                return Ok(studentDtos);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var studentDto = await _studentService.GetCurrentStudentAsync(HttpContext);
                return Ok(studentDto);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchStudents(
            [FromQuery] string? searchTerm = null,
            [FromQuery] Guid? majorId = null,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            try
            {
                var results = await _studentService.SearchStudentsAsync(
                    searchTerm,
                    majorId,
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
        public async Task<IActionResult> CreateStudent([FromBody] CreateStudentDto createStudentDto)
        {
            try
            {
                var studentDto = await _studentService.CreateStudentAsync(createStudentDto,HttpContext);
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

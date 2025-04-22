using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected IActionResult HandleException(Exception ex)
        {
            if (ex is KeyNotFoundException)
            {
                return NotFound(new { Message = ex.Message });
            }
            else if (ex is ArgumentException)
            {
                return BadRequest(new { Message = ex.Message });
            }
            else
            {
                return StatusCode(500, new { Message = "An unexpected error occurred.", Details = ex.Message });
            }
        }
    }
}

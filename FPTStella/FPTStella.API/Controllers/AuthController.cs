using FPTStella.Application.Common.Interfaces.Jwt;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.UseCases.Auth;
using FPTStella.Contracts.DTOs.Google;
using Google.Apis.Auth.OAuth2.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GoogleLoginUseCase _googleLoginUseCase;

        public AuthController(GoogleLoginUseCase googleLoginUseCase)
        {
            _googleLoginUseCase = googleLoginUseCase;
        }

        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequest request)
        {
            try
            {
                var response = await _googleLoginUseCase.ExecuteAsync(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                var jwtService = HttpContext.RequestServices.GetRequiredService<IJwtService>();
                var (accessToken, refreshToken) = await jwtService.RefreshTokenAsync(request.RefreshToken);
                return Ok(new { accessToken, refreshToken });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        public class RefreshTokenRequest
        {
            public string RefreshToken { get; set; }
        }
    }
}

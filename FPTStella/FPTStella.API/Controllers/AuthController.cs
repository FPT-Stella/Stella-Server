using FPTStella.Application.Common.Interfaces.Google;
using FPTStella.Application.Common.Interfaces.Jwt;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.UseCases.Auth;
using FPTStella.Contracts.DTOs.Google;
using Google.Apis.Auth.OAuth2.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using FPTStella.Contracts.DTOs.Accounts;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly GoogleLoginUseCase _googleLoginUseCase;
        private readonly IAccountService _accountService;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public AuthController(
            GoogleLoginUseCase googleLoginUseCase,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IAccountService accountService)
        {
            _googleLoginUseCase = googleLoginUseCase ?? throw new ArgumentNullException(nameof(googleLoginUseCase));
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _accountService = accountService;
        }
        [HttpPost("admin-login")]
        public async Task<IActionResult> AdminLogin([FromBody] AdminLoginRequest request)
        {
            try
            {
                var userWithTokenDto = await _accountService.AdminLoginAsync(request.Email, request.Password);
                return Ok(userWithTokenDto);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("oauth/google")]
        public async Task<IActionResult> GoogleOAuth([FromBody] GoogleGetCodeDto codeDto)
        {
            try
            {
                // Log the received code
                Console.WriteLine($"Received code from frontend: {codeDto?.code}");

                if (string.IsNullOrEmpty(codeDto?.code))
                {
                    return BadRequest(new { error = "Authorization code is required" });
                }

                // Step 1: Exchange the authorization code for Google tokens
                var client = _httpClientFactory.CreateClient();

                // Decode code if it contains URL-encoded characters
                var decodedCode = Uri.UnescapeDataString(codeDto.code);
                Console.WriteLine($"Decoded code: {decodedCode}");

                var formData = new[]
                {
                    new KeyValuePair<string, string>("code", decodedCode),
                    new KeyValuePair<string, string>("client_id", _configuration["Google:ClientId"]),
                    new KeyValuePair<string, string>("client_secret", _configuration["Google:ClientSecret"]),
                     new KeyValuePair<string, string>("redirect_uri", "http://localhost:5173/oauth/google/callback"),
                    new KeyValuePair<string, string>("grant_type", "authorization_code")
                };
                Console.WriteLine("======== GOOGLE OAUTH REQUEST ========");
                Console.WriteLine($"Code: {decodedCode}");
                Console.WriteLine($"Client ID: {_configuration["Google:ClientId"]}");
                Console.WriteLine($"Client Secret: {_configuration["Google:ClientSecret"]}");
                Console.WriteLine($"Redirect URI: {_configuration["Google:RedirectUri"]}");
                Console.WriteLine("=======================================");

                var formContent = new FormUrlEncodedContent(formData);

                // Set proper Content-Type header
                formContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");

                var tokenResponse = await client.PostAsync(
                    "https://oauth2.googleapis.com/token",
                    formContent);

                var responseContent = await tokenResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"Response status: {tokenResponse.StatusCode}");
                Console.WriteLine($"Response body: {responseContent}");

                if (!tokenResponse.IsSuccessStatusCode)
                {
                    return BadRequest(new { error = $"Failed to exchange code: {responseContent}" });
                }

                // Step 2: Parse the token response from Google
                var googleTokens = JsonSerializer.Deserialize<GoogleTokensResponse>(
                    responseContent,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (string.IsNullOrEmpty(googleTokens?.id_token))
                {
                    return BadRequest(new { error = "ID token not returned from Google" });
                }

                // Step 3: Use our existing flow to validate the ID token and get user information
                var loginRequest = new GoogleLoginRequest { IdToken = googleTokens.id_token };
                var loginResponse = await _googleLoginUseCase.ExecuteAsync(loginRequest);

                return Ok(loginResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
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

        private class GoogleTokensResponse
        {
            public string access_token { get; set; } = string.Empty;
            public string id_token { get; set; } = string.Empty;
            public string refresh_token { get; set; } = string.Empty;
            public int expires_in { get; set; }
            public string token_type { get; set; } = string.Empty;
        }
    }
}

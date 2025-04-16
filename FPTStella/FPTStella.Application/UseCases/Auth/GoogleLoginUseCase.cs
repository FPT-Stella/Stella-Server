using FPTStella.Application.Common.Interfaces.Google;
using FPTStella.Application.Common.Interfaces.Jwt;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.Google;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.UseCases.Auth
{
    public class GoogleLoginUseCase
    {
        private readonly IGoogleAuthService _googleAuthService;
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        public GoogleLoginUseCase(
            IGoogleAuthService googleAuthService,
            IUserService userService,
            IJwtService jwtService,
            IConfiguration configuration)
        {
            _googleAuthService = googleAuthService;
            _userService = userService;
            _jwtService = jwtService;
            _configuration = configuration;
        }

        public async Task<GoogleLoginResponse> ExecuteAsync(GoogleLoginRequest request)
        {
            // Xác thực Google id_token
            var tokenInfo = await _googleAuthService.ValidateGoogleTokenAsync(request.IdToken);

            // Kiểm tra aud
            var expectedAudience = _configuration["Google:ClientId"];
            if (tokenInfo.Aud != expectedAudience)
            {
                throw new Exception("Invalid audience in id_token");
            }

            // Tìm hoặc tạo user
            var userDto = await _userService.FindOrCreateGoogleUserAsync(tokenInfo.Email, tokenInfo.Name);

            // Sinh token
            var accessToken = _jwtService.GenerateAccessToken(userDto.Email, userDto.Role);
            var refreshToken = _jwtService.GenerateRefreshToken(userDto.Email, userDto.Role);

            return new GoogleLoginResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Username = userDto.Username,
                Email = userDto.Email,
                Role = userDto.Role
            };
        }
    }
}

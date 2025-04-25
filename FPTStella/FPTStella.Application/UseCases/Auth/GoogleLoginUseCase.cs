using FPTStella.Application.Common.Interfaces.Google;
using FPTStella.Application.Common.Interfaces.Jwt;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.Google;
using FPTStella.Domain.Entities;
using FPTStella.Domain.Enums;
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
        private readonly IAccountService _userService;
        private readonly IStudentService _studentService;
        private readonly IJwtService _jwtService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public GoogleLoginUseCase(
            IGoogleAuthService googleAuthService,
            IAccountService userService,
            IJwtService jwtService,
            IConfiguration configuration,
            IStudentService studentService, IUnitOfWork unitOfWork)
        {
            _googleAuthService = googleAuthService;
            _userService = userService;
            _jwtService = jwtService;
            _configuration = configuration;
            _studentService = studentService;
            _unitOfWork = unitOfWork;
        }

        public async Task<GoogleLoginResponse> ExecuteAsync(GoogleLoginRequest request)
        {
            // Xác thực Google id_token
            var tokenInfo = await _googleAuthService.ValidateGoogleTokenAsync(request.IdToken);
            var studentRepo = _unitOfWork.Repository<Student>();
            // Kiểm tra aud
            var expectedAudience = _configuration["Google:ClientId"];
            if (tokenInfo.Aud != expectedAudience)
            {
                throw new Exception("Invalid audience in id_token");
            }

            // Tìm hoặc tạo user
            var userDto = await _userService.FindOrCreateGoogleUserAsync(tokenInfo.Email, tokenInfo.Name);
            var acc = new Account
            {
                Id = Guid.Parse(userDto.Id), // Fix: Convert string to Guid using Guid.Parse
                Username = userDto.Username,
                Email = userDto.Email,
                Role = Enum.TryParse<Role>(userDto.Role, out var parsedRole) ? parsedRole : throw new Exception("Invalid role") // Fix: Convert string to Role enum
            };
            var existingStudent = await _studentService.GetStudentByUserIdAsync(acc.Id.ToString());
            if (existingStudent == null)
            {
                var student = new Student
            {
                UserId = acc.Id,
                MajorId = Guid.Empty, 
                StudentCode = "",
                Phone = "", 
                Address = "" 
            };
                await studentRepo.InsertAsync(student);
                await _unitOfWork.SaveAsync(); 
            }

            // Sinh token
            var accessToken = _jwtService.GenerateJwtToken(acc);
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

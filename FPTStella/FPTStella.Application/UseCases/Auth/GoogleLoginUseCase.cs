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
        private readonly IEmailService _emailService;

        public GoogleLoginUseCase(
            IGoogleAuthService googleAuthService,
            IAccountService userService,
            IJwtService jwtService,
            IConfiguration configuration,
            IStudentService studentService, IUnitOfWork unitOfWork,IEmailService emailService)
        {
            _googleAuthService = googleAuthService;
            _userService = userService;
            _jwtService = jwtService;
            _configuration = configuration;
            _studentService = studentService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
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
                var body = $@"<html lang=""en"">
<head>
    <meta charset=""UTF-8"">
    <title>Welcome to Our Platform</title>
    <style>
        body {{
            font-family: 'Segoe UI', sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 40px auto;
            background-color: #ffffff;
            border-radius: 10px;
            overflow: hidden;
            box-shadow: 0 4px 10px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            background-color: #4285f4;
            color: white;
            padding: 20px;
            text-align: center;
        }}
        .header h1 {{
            margin: 0;
            font-size: 24px;
        }}
        .content {{
            padding: 30px;
            color: #333;
        }}
        .content p {{
            line-height: 1.6;
        }}
        .footer {{
            background-color: #f1f1f1;
            text-align: center;
            padding: 15px;
            font-size: 12px;
            color: #888;
        }}
        .button {{
            display: inline-block;
            margin-top: 20px;
            padding: 12px 24px;
            background-color: #34a853;
            color: white;
            text-decoration: none;
            border-radius: 6px;
            font-weight: bold;
        }}
    </style>
</head>
<body>
<div class=""container"">
    <div class=""header"">
        <h1>🎉 Welcome, {acc.Username}!</h1>
    </div>
    <div class=""content"">
        <p>Hi <strong>{acc.Username}</strong>,</p>
        <p>You've successfully logged in with your Google account for the first time. Your account has been created and is ready to use.</p>

        <p>Here are your details:</p>
        <ul>
            <li><strong>Email:</strong> {acc.Email}</li>
            <li><strong>Role:</strong> {acc.Role}</li>
        </ul>

        <p>If you have any questions, feel free to contact our support team.</p>

        <p>Best regards,<br>The Stella Team</p>
    </div>
    <div class=""footer"">
        © 2025 Stella. All rights reserved.
    </div>
</div>
</body>
</html>";
                await _emailService.SendEmailAsyncMailJet(acc.Email, "Welcome to Our Platform", body);
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

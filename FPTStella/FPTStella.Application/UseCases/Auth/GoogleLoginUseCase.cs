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
                Id = Guid.Parse(userDto.Id),
                Username = userDto.Username,
                Email = userDto.Email,
                Role = Enum.TryParse<Role>(userDto.Role, out var parsedRole) ? parsedRole : throw new Exception("Invalid role")
            };
            var existingStudent = await studentRepo.FindOneAsync(s => s.UserId == acc.Id);

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
                var body = $@"<!DOCTYPE html>
                    <html lang=""en"">
                    <head>
                        <meta charset=""UTF-8"">
                        <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                        <title>Welcome to FPT Stella</title>
                        <style>
                            @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap');
        
                            * {{
                                margin: 0;
                                padding: 0;
                                box-sizing: border-box;
                            }}
        
                            body {{
                                font-family: 'Poppins', 'Segoe UI', sans-serif;
                                background-color: #f5f5f5;
                                color: #333333;
                                line-height: 1.6;
                            }}
        
                            .email-wrapper {{
                                max-width: 600px;
                                margin: 0 auto;
                                background-color: #ffffff;
                            }}
        
                            .email-header {{
                                background-color: #635BFF;
                                padding: 30px 0;
                                text-align: center;
                            }}
        
                            .email-header img {{
                                height: 60px;
                                margin-bottom: 15px;
                            }}
        
                            .email-header h1 {{
                                color: white;
                                font-size: 26px;
                                font-weight: 600;
                                margin: 0;
                            }}
        
                            .email-body {{
                                padding: 40px 30px;
                            }}
        
                            .greeting {{
                                font-size: 22px;
                                font-weight: 500;
                                margin-bottom: 20px;
                                color: #333333;
                            }}
        
                            .message {{
                                margin-bottom: 30px;
                                font-size: 16px;
                            }}
        
                            .user-details {{
                                background-color: #f9f9f9;
                                border-radius: 8px;
                                padding: 20px 25px;
                                margin: 25px 0;
                            }}
        
                            .user-details h3 {{
                                color: #837df8;
                                font-size: 18px;
                                margin-bottom: 15px;
                                font-weight: 500;
                            }}
        
                            .user-details-item {{
                                display: flex;
                                margin-bottom: 12px;
                            }}
        
                            .detail-label {{
                                font-weight: 600;
                                width: 100px;
                            }}
        
                            .action-btn {{
                                display: block;
                                background-color: #635BFF;
                                color: white !important;
                                text-decoration: none;
                                padding: 14px 28px;
                                border-radius: 6px;
                                font-weight: 500;
                                margin: 25px 0;
                                text-align: center;
                                width: 100%;
                                font-size: 16px;
                            }}
        
                            .divider {{
                                height: 1px;
                                background-color: #e1e1e1;
                                margin: 30px 0;
                            }}
        
                            .email-footer {{
                                background-color: #f0f0f0;
                                padding: 25px;
                                text-align: center;
                            }}
        
                            .social-icons {{
                                margin: 15px 0;
                            }}
        
                            .social-icons a {{
                                display: inline-block;
                                margin: 0 8px;
                            }}
        
                            .social-icons img {{
                                width: 24px;
                                height: 24px;
                            }}
        
                            .footer-links {{
                                margin: 15px 0;
                            }}
        
                            .footer-links a {{
                                color: #555555;
                                text-decoration: none;
                                margin: 0 10px;
                                font-size: 14px;
                            }}
        
                            .copyright {{
                                color: #777777;
                                font-size: 13px;
                                margin-top: 20px;
                            }}
                        </style>
                    </head>
                    <body>
                        <div class=""email-wrapper"">
                            <!-- Header -->
                            <div class=""email-header"">
                                <h1>✨ Welcome to FPT Stella ✨</h1>
                            </div>
        
                            <!-- Body -->
                            <div class=""email-body"">
                                <div class=""greeting"">Hello, {acc.Username}! 👋</div>
            
                                <div class=""message"">
                                    Thank you for joining FPT Stella! Your account has been successfully created after logging in with Google.
                                </div>
            
                                <div class=""user-details"">
                                    <h3>Your Account Information</h3>
                                    <div class=""user-details-item"">
                                        <div class=""detail-label"">Name:</div>
                                        <div>{acc.FullName}</div>
                                    </div>
                                    <div class=""user-details-item"">
                                        <div class=""detail-label"">Email:</div>
                                        <div>{acc.Email}</div>
                                    </div>
                                    <div class=""user-details-item"">
                                        <div class=""detail-label"">Role:</div>
                                        <div>{acc.Role}</div>
                                    </div>
                                </div>
            
                                <div class=""message"">
                                    You can now access all the features and resources available to you in the Stella platform. We're excited to have you join our community!
                                </div>
            
                                <a href=""https://stella.fpt.edu.vn/dashboard"" class=""action-btn"">Access Your Dashboard</a>
            
                                <div class=""divider""></div>
            
                                <div class=""message"">
                                    If you have any questions or need assistance, don't hesitate to contact our support team at <strong>support@stella.fpt.edu.vn</strong>.
                                </div>
            
                                <div class=""message"">
                                    Best regards,<br>
                                    <strong>The FPT Stella Team 🌠</strong>
                                </div>
                            </div>
        
                            <!-- Footer -->
                            <div class=""email-footer"">
                                <div class=""social-icons"">
                                    <a href=""https://facebook.com/fptstella""><img src=""https://cdn-icons-png.flaticon.com/512/733/733547.png"" alt=""Facebook""></a>
                                </div>
            
                                <div class=""footer-links"">
                                    <a href=""https://stella.fpt.edu.vn/help"">Help Center</a>
                                </div>
            
                                <div class=""copyright"">
                                    © 2025 FPT Stella. All rights reserved.
                                </div>
                            </div>
                        </div>
                    </body>
                    </html>";
                await _emailService.SendEmailAsyncMailJet(acc.Email, "✨ Welcome to FPT Stella ✨", body);
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

using FPTStella.Application.Common.Interfaces.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;

        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateAccessToken(string email, string role)
        {
            return GenerateToken(email, role, "access");
        }

        public string GenerateRefreshToken(string email, string role)
        {
            return GenerateToken(email, role, "refresh");
        }
        public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var refreshSecret = _configuration["JwtSettings:RefreshSecretToken"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];

            var principal = tokenHandler.ValidateToken(refreshToken, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(refreshSecret))
            }, out var validatedToken);

            var email = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            var role = principal.FindFirst(ClaimTypes.Role)?.Value;

            var newAccessToken = GenerateToken(email, role, "access");
            var newRefreshToken = GenerateToken(email, role, "refresh");

            return (newAccessToken, newRefreshToken);
        }
        private string GenerateToken(string email, string role, string tokenType)
        {
            var secret = tokenType == "access"
                ? _configuration["JwtSettings:AccessSecretToken"]
                : _configuration["JwtSettings:RefreshSecretToken"];
            var issuer = _configuration["JwtSettings:Issuer"];
            var audience = _configuration["JwtSettings:Audience"];
            var expiresInMinutes = tokenType == "access"
                ? int.Parse(_configuration["JwtSettings:AccessTokenExpMinute"])
                : int.Parse(_configuration["JwtSettings:RefreshTokenExpMinute"]);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(ClaimTypes.Name, email.Split('@')[0]),
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.Role, role),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("type", tokenType)
            };

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(expiresInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

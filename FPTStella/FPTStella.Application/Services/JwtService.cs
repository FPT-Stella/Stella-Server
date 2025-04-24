using FPTStella.Application.Common.Interfaces.Jwt;
using FPTStella.Contracts.DTOs.Jwt;
using FPTStella.Domain.Entities;
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
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GenerateAccessToken(string email, string role)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role cannot be null or empty.", nameof(role));

            return GenerateToken(email, role, "access");
        }

        public string GenerateRefreshToken(string email, string role)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role cannot be null or empty.", nameof(role));

            return GenerateToken(email, role, "refresh");
        }

        public async Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken)) throw new ArgumentException("Refresh token cannot be null or empty.", nameof(refreshToken));

            var tokenHandler = new JwtSecurityTokenHandler();
            var refreshSecret = _configuration["JwtSettings:RefreshSecretToken"] ?? throw new InvalidOperationException("Refresh secret token is not configured.");
            var issuer = _configuration["JwtSettings:Issuer"] ?? throw new InvalidOperationException("Issuer is not configured.");
            var audience = _configuration["JwtSettings:Audience"] ?? throw new InvalidOperationException("Audience is not configured.");

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

            var email = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value ?? throw new SecurityTokenException("Email claim is missing.");
            var role = principal.FindFirst(ClaimTypes.Role)?.Value ?? throw new SecurityTokenException("Role claim is missing.");

            // Example of an asynchronous operation (e.g., database call)
            await Task.Delay(10); // Simulate async work

            var newAccessToken = GenerateToken(email, role, "access");
            var newRefreshToken = GenerateToken(email, role, "refresh");

            return (newAccessToken, newRefreshToken);
        }


        private string GenerateToken(string email, string role, string tokenType)
        {
            if (string.IsNullOrWhiteSpace(email)) throw new ArgumentException("Email cannot be null or empty.", nameof(email));
            if (string.IsNullOrWhiteSpace(role)) throw new ArgumentException("Role cannot be null or empty.", nameof(role));
            if (string.IsNullOrWhiteSpace(tokenType)) throw new ArgumentException("Token type cannot be null or empty.", nameof(tokenType));

            var secret = tokenType == "access"
                ? _configuration["JwtSettings:AccessSecretToken"]
                : _configuration["JwtSettings:RefreshSecretToken"];

            if (string.IsNullOrWhiteSpace(secret)) throw new InvalidOperationException($"Secret for {tokenType} token is not configured.");

            var issuer = _configuration["JwtSettings:Issuer"] ?? throw new InvalidOperationException("Issuer is not configured.");
            var audience = _configuration["JwtSettings:Audience"] ?? throw new InvalidOperationException("Audience is not configured.");

            var expiresInMinutesConfig = tokenType == "access"
                ? _configuration["JwtSettings:AccessTokenExpMinute"]
                : _configuration["JwtSettings:RefreshTokenExpMinute"];

            if (!int.TryParse(expiresInMinutesConfig, out var expiresInMinutes))
            {
                throw new InvalidOperationException($"Invalid expiration time for {tokenType} token.");
            }

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
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        public string GenerateJwtToken(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (string.IsNullOrWhiteSpace(account.Email)) throw new ArgumentException("Email is required.");
            if (account.Role == null) throw new ArgumentException("Role is required."); // Updated to check for null Role enum

            var secret = _configuration["JwtSettings:AccessSecretToken"];
            if (string.IsNullOrWhiteSpace(secret)) throw new InvalidOperationException("Access token secret is not configured.");

            var issuer = _configuration["JwtSettings:Issuer"] ?? throw new InvalidOperationException("Issuer is not configured.");
            var audience = _configuration["JwtSettings:Audience"] ?? throw new InvalidOperationException("Audience is not configured.");
            var lifetime = _configuration["JwtSettings:AccessTokenExpMinute"];

            if (!int.TryParse(lifetime, out var expiresInMinutes))
            {
                throw new InvalidOperationException("Invalid access token lifetime.");
            }

            var claims = new[]
            {
                new Claim("id", account.Id.ToString()),
                new Claim("email", account.Email),
                new Claim("role", account.Role.ToString()), // Convert Role enum to string
                new Claim("username", account.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("type", "access")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresInMinutes),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}

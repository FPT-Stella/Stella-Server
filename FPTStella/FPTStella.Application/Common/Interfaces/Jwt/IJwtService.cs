using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Jwt
{
    public interface IJwtService
    {
        string GenerateAccessToken(string email, string role);
        string GenerateJwtToken(Account account);
        string GenerateRefreshToken(string email, string role);
        Task<(string AccessToken, string RefreshToken)> RefreshTokenAsync(string refreshToken);
    }
}

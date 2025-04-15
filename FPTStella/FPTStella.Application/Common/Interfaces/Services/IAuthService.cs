using FPTStella.Contracts.DTOs.Google;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IAuthService
    {
        Task<TokenResponseDto> GoogleLoginAsync(GoogleLoginDto googleLoginDto);
    }
}

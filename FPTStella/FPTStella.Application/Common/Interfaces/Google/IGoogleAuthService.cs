using FPTStella.Contracts.DTOs.Google;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Google
{
    public interface IGoogleAuthService
    {
        Task<GoogleTokenInfo> ValidateGoogleTokenAsync(string idToken);
    }
}

using FPTStella.Application.Common.Interfaces.Google;
using FPTStella.Contracts.DTOs.Google;
using Google.Apis.Auth;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private readonly IConfiguration _configuration;

        public GoogleAuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<GoogleTokenInfo> ValidateGoogleTokenAsync(string idToken)
        {
            try
            {
                var settings = new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { _configuration["Google:ClientId"] }
                };

                var payload = await GoogleJsonWebSignature.ValidateAsync(idToken, settings);

                return new GoogleTokenInfo
                {
                    Aud = payload.Audience.ToString(),
                    Email = payload.Email,
                    Name = payload.Name
                };
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid Google token", ex);
            }
        }
    }
}

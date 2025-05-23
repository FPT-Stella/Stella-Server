﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Jwt
{
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public double Lifetime { get; set; }
        public string RefreshSecretToken { get; set; }
        public double RefreshTokenExpMinute { get; set; }
        public string AccessSecretToken { get; set; }
        public double AccessTokenExpMinute { get; set; }
    }
}

﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Google
{
    public class GoogleLoginDto
    {
        [Required]
        public string IdToken { get; set; }
    }
}

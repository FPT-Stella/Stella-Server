﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.POs
{
    public class CreatePOsDto
    {
        public Guid ProgramId { get; set; }
        public string PoName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Tools
{
    public class CreateToolDto
    {
        public required string ToolName { get; set; } = string.Empty;
        public required string Description { get; set; } = string.Empty;
    }
}

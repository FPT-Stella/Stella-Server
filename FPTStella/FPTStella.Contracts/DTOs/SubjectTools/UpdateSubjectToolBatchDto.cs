﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectTools
{
    public class UpdateSubjectToolBatchDto
    {
        public List<UpdateSubjectToolDto> Mappings { get; set; } = new List<UpdateSubjectToolDto>();
    }
}

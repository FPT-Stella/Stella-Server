﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectComboSubjects
{
    public class UpdateSubjectComboSubjectDto
    {
        public Guid SubjectComboId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid? NewSubjectComboId { get; set; }
        public Guid? NewSubjectId { get; set; }
    }
}

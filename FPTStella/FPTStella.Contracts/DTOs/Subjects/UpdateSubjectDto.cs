﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Subjects
{
    public class UpdateSubjectDto
    {
        public  string SubjectName { get; set; }
        public  string SubjectCode { get; set; }
        public  string SubjectDescription { get; set; } = string.Empty;
        public  int Credits { get; set; }
        public bool Prerequisite { get; set; }
        public  string PrerequisiteName { get; set; } = string.Empty;
        public  string DegreeLevel { get; set; } = string.Empty;
        public  string TimeAllocation { get; set; } = string.Empty;
        public  string SysllabusDescription { get; set; } = string.Empty;
        public  string StudentTask { get; set; } = string.Empty;
        public int ScoringScale { get; set; }
        public int MinAvgMarkToPass { get; set; }
        public  string Note { get; set; } = string.Empty;
        public  string Topic { get; set; } = string.Empty;
        public bool LearningTeachingType { get; set; }
        public int TermNo { get; set; }
    }
}

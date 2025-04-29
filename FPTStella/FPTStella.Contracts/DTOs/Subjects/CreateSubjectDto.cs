using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Subjects
{
    public class CreateSubjectDto
    {
        public required string SubjectName { get; set; }
        public required string SubjectCode { get; set; }
        public required string SubjectDescription { get; set; } = string.Empty;
        public required int Credits { get; set; }
        public required bool Prerequisite { get; set; }
        public required string PrerequisiteName { get; set; } = string.Empty;
        public required string DegreeLevel { get; set; } = string.Empty;
        public required string TimeAllocation { get; set; } = string.Empty;
        public required string SysllabusDescription { get; set; } = string.Empty;
        public required string StudentTask { get; set; } = string.Empty;
        public required int ScoringScale { get; set; }
        public required int MinAvgMarkToPass { get; set; }
        public required string Note { get; set; } = string.Empty;
        public required string Topic { get; set; } = string.Empty;
        public required bool LearningTeachingType { get; set; }
        public required int TermNo { get; set; }
    }
}

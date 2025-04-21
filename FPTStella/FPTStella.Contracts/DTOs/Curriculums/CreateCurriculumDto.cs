using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Curriculums
{
    public class CreateCurriculumDto
    {
        public required Guid ProgramId { get; set; }
        public required string CurriculumCode { get; set; }
        public required string CurriculumName { get; set; }
        public string Description { get; set; } = string.Empty;
        public int? TotalCredit { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}

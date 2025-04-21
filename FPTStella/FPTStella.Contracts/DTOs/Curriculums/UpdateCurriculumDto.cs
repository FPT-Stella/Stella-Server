using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Curriculums
{
    public class UpdateCurriculumDto
    {
        public Guid? ProgramId { get; set; }
        public string? CurriculumCode { get; set; }
        public string? CurriculumName { get; set; }
        public string? Description { get; set; }
        public int? TotalCredit { get; set; }
        public int? StartYear { get; set; }
        public int? EndYear { get; set; }
    }
}

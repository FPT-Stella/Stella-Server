using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectCombos
{
    public class CreateSubjectComboDto
    {
        public Guid ProgramId { get; set; }
        public required string ComboName { get; set; }
        public string Description { get; set; } = string.Empty;
        public string ProgramOutcome { get; set; } = string.Empty;
    }
}

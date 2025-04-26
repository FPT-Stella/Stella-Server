using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectCombos
{
    public class SubjectComboDto
    {
        public Guid Id { get; set; }
        public Guid ProgramId { get; set; }
        public string ComboName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ProgramOutcome { get; set; } = string.Empty;
    }
}

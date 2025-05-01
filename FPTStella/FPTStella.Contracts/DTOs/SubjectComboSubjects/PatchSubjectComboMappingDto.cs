using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectComboSubjects
{
    public class PatchSubjectComboMappingDto
    {
        public Guid SubjectComboId { get; set; }
        public List<Guid> SubjectIds { get; set; } = new List<Guid>();
    }
}

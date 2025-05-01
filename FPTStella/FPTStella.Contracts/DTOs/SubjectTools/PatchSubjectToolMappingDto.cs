using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectTools
{
    public class PatchSubjectToolMappingDto
    {
        public Guid SubjectId { get; set; }
        public List<Guid> ToolIds { get; set; } = new List<Guid>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectTools
{
    public class PatchToolSubjectMappingDto
    {
        public Guid ToolId { get; set; }
        public List<Guid> SubjectIds { get; set; } = new List<Guid>();
    }
}

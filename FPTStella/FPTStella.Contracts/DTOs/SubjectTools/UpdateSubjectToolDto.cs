using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectTools
{
    public class UpdateSubjectToolDto
    {
        public Guid SubjectId { get; set; }
        public Guid ToolId { get; set; }
        public Guid? NewSubjectId { get; set; }
        public Guid? NewToolId { get; set; }
    }
}

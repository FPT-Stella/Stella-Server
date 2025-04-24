using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Programs
{
    public class UpdateProgramDto
    {
        public  Guid? MajorId { get; set; }
        public  string? ProgramCode { get; set; }
        public  string? ProgramName { get; set; }
        public  string? Description { get; set; } = string.Empty;
    }
}

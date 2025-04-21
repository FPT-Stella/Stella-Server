using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Programs
{
    public class ProgramDto
    {
        public required string Id { get; set; }
        public required string MajorId { get; set; }
        public required string ProgramCode { get; set; }
        public required string ProgramName { get; set; }
        public required string Description { get; set; } = string.Empty;
    }
}

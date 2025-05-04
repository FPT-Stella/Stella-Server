using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.CLOs
{
    public class CreateCLODto
    {
        public Guid SubjectId { get; set; }
        public string CloName { get; set; } = string.Empty;
        public string CloDetails { get; set; } = string.Empty;
        public string LoDetails { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectTools
{
    public class ToolWithNameDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Tools
{
    public class ToolDto
    {
        public Guid Id { get; set; }
        public string ToolName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public bool DelFlg { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}

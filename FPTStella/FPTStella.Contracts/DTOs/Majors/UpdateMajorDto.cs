using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Majors
{
    public class UpdateMajorDto
    {
        public string MajorName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

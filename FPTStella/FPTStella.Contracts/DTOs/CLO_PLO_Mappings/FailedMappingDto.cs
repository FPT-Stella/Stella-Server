using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.CLO_PLO_Mappings
{
    public class FailedMappingDto
    {
        public UpdateCLO_PLO_MappingDto Mapping { get; set; }
        public string Reason { get; set; } = string.Empty;
    }
}

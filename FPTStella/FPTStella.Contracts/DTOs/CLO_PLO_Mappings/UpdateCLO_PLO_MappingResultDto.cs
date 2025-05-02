using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.CLO_PLO_Mappings
{
    public class UpdateCLO_PLO_MappingResultDto
    {
        public List<UpdateCLO_PLO_MappingDto> UpdatedMappings { get; set; } = new List<UpdateCLO_PLO_MappingDto>();
        public List<FailedMappingDto> FailedMappings { get; set; } = new List<FailedMappingDto>();
    }
}

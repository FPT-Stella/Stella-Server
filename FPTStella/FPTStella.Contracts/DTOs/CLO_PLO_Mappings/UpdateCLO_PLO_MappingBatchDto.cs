using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.CLO_PLO_Mappings
{
    public class UpdateCLO_PLO_MappingBatchDto
    {
        public List<UpdateCLO_PLO_MappingDto> Mappings { get; set; } = new List<UpdateCLO_PLO_MappingDto>();
    }
}

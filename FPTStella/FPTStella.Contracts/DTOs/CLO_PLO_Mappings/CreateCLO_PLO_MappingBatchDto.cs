using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.CLO_PLO_Mappings
{
    public class CreateCLO_PLO_MappingBatchDto
    {
        public List<CreateCLO_PLO_MappingDto> Mappings { get; set; } = new List<CreateCLO_PLO_MappingDto>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.PO_PLO_Mappings
{
    public class CreatePO_PLO_MappingBatchDto
    {
        public List<CreatePO_PLO_MappingDto> Mappings { get; set; } = new List<CreatePO_PLO_MappingDto>();
    }
}

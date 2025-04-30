using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.PO_PLO_Mappings
{
    public class UpdatePO_PLO_MappingBatchDto
    {
        public List<UpdatePO_PLO_MappingDto> Mappings { get; set; }

    }
    public class UpdatePO_PLO_MappingDto
    {
        public Guid PoId { get; set; }
        public Guid PloId { get; set; }
        public Guid? NewPoId { get; set; } 
        public Guid? NewPloId { get; set; } 
    }
    public class UpdatePO_PLO_MappingResultDto
    {
        public List<UpdatePO_PLO_MappingDto> UpdatedMappings { get; set; }
        public List<FailedMappingDto> FailedMappings { get; set; }
    }

    public class FailedMappingDto
    {
        public UpdatePO_PLO_MappingDto Mapping { get; set; }
        public string Reason { get; set; }
    }
}

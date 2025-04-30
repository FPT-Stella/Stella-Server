using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.PO_PLO_Mappings
{
    public class PatchPloMappingDto
    {
        public Guid PloId { get; set; }
        public List<Guid> PoIds { get; set; }
    }

}

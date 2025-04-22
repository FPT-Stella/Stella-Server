using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.PO_PLO_Mappings
{
    public class CreatePO_PLO_MappingDto
    {
        public Guid PoId { get; set; }
        public Guid PloId { get; set; }
    }
}

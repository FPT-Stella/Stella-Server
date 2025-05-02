using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.CLO_PLO_Mappings
{
    public class PatchCloPloMappingDto
    {
        public Guid PloId { get; set; }
        public List<Guid> CloIds { get; set; } = new List<Guid>();
    }
}

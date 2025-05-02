using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.CLO_PLO_Mappings
{
    public class UpdateCLO_PLO_MappingDto
    {
        public Guid CloId { get; set; }
        public Guid PloId { get; set; }
        public Guid? NewCloId { get; set; }
        public Guid? NewPloId { get; set; }
    }
}

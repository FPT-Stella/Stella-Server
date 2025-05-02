using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.CLO_PLO_Mappings
{
    public class PatchCloMappingDto
    {
        /// <summary>
        /// The ID of the CLO to update mappings for
        /// </summary>
        public Guid CloId { get; set; }

        /// <summary>
        /// The list of PLO IDs to associate with the CLO
        /// </summary>
        public List<Guid> PloIds { get; set; } = new List<Guid>();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.PO_PLO_Mappings
{
    public class PatchPoMappingDto
    {
        /// <summary>
        /// The ID of the PO to update mappings for
        /// </summary>
        public Guid PoId { get; set; }

        /// <summary>
        /// The list of PLO IDs to associate with the PO
        /// </summary>
        public List<Guid> PloIds { get; set; } = new List<Guid>();
    }
}

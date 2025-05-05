using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.PLOs
{
    public class PLOsDto
    {
        public Guid Id { get; set; }
        public Guid CurriculumId { get; set; }
        public string CurriculumCode { get; set; } = string.Empty;
        public string PloName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

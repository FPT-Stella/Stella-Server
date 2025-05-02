using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.CLOs
{
    public class CLOWithDetailsDto
    {
        public Guid Id { get; set; }
        public string Details { get; set; } = string.Empty;
    }
}

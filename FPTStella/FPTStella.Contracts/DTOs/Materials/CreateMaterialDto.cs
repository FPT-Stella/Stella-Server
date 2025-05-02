using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Materials
{
    public class CreateMaterialDto
    {
        public required string SubjectId { get; set; }
        public required string MaterialName { get; set; }
        public required string MaterialType { get; set; }
        public required string MaterialUrl { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

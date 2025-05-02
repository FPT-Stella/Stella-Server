using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Materials
{
    public class UpdateMaterialDto
    {
        public string MaterialName { get; set; } = string.Empty;
        public string MaterialType { get; set; } = string.Empty;
        public string MaterialUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}

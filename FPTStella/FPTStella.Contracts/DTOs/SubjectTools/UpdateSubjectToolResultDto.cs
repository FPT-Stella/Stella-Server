using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectTools
{
    public class UpdateSubjectToolResultDto
    {
        public List<UpdateSubjectToolDto> UpdatedMappings { get; set; } = new List<UpdateSubjectToolDto>();
        public List<FailedSubjectToolMappingDto> FailedMappings { get; set; } = new List<FailedSubjectToolMappingDto>();
    }

    public class FailedSubjectToolMappingDto
    {
        public UpdateSubjectToolDto Mapping { get; set; }
        public string Reason { get; set; }
    }
}

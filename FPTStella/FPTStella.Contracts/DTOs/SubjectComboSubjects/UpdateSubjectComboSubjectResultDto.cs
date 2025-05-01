using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectComboSubjects
{
    public class UpdateSubjectComboSubjectResultDto
    {
        public List<UpdateSubjectComboSubjectDto> UpdatedMappings { get; set; } = new List<UpdateSubjectComboSubjectDto>();
        public List<FailedMappingDto> FailedMappings { get; set; } = new List<FailedMappingDto>();
    }

    public class FailedMappingDto
    {
        public UpdateSubjectComboSubjectDto Mapping { get; set; }
        public string Reason { get; set; }
    }
}

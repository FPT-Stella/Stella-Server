using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectComboSubjects
{
    public class UpdateSubjectComboSubjectBatchDto
    {
        public List<UpdateSubjectComboSubjectDto> Mappings { get; set; } = new List<UpdateSubjectComboSubjectDto>();
    }
}

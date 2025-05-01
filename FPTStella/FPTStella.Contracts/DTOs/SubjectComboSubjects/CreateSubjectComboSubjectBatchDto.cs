using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectComboSubjects
{
    public class CreateSubjectComboSubjectBatchDto
    {
        public List<CreateSubjectComboSubjectDto> Mappings { get; set; } = new List<CreateSubjectComboSubjectDto>();
    }
}

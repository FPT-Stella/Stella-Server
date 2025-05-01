using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectTools
{
    public class CreateSubjectToolBatchDto
    {
        public List<CreateSubjectToolDto> Mappings { get; set; } = new List<CreateSubjectToolDto>();
    }
}

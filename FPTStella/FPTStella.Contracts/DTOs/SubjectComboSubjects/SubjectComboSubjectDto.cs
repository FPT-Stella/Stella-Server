using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectComboSubjects
{
    public class SubjectComboSubjectDto
    {       
        public Guid Id { get; set; }      
        public Guid SubjectComboId { get; set; }
        public Guid SubjectId { get; set; }
    }
}

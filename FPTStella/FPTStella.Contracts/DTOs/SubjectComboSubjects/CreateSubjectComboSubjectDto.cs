using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectComboSubjects
{
    public class CreateSubjectComboSubjectDto
    {
        /// <summary>
        /// Gets or sets the ID of the subject combo
        /// </summary>
        public Guid SubjectComboId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the subject
        /// </summary>
        public Guid SubjectId { get; set; }
    }
}

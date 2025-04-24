using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectInCurriculums
{
    public class CreateSubjectInCurriculumDto
    {
        /// <summary>
        /// Gets or sets the curriculum ID.
        /// </summary>
        public Guid CurriculumId { get; set; }
        /// <summary>
        /// Gets or sets the subject ID.
        /// </summary>
        public Guid SubjectId { get; set; }
    }
}

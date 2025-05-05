using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Dashboard
{
    public class ComboSubjectCountDto
    {
        /// <summary>
        /// Gets or sets the combo subject ID.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the combo subject name.
        /// </summary>
        public string ComboName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of subjects in this combo.
        /// </summary>
        public int SubjectCount { get; set; }
    }
}

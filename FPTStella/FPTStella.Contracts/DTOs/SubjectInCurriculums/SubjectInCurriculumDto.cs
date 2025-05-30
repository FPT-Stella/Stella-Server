﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.SubjectInCurriculums
{
    public class SubjectInCurriculumDto
    {
        /// <summary>
        /// Gets or sets the ID of the mapping.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the curriculum ID.
        /// </summary>
        public Guid CurriculumId { get; set; }

        /// <summary>
        /// Gets or sets the subject ID.
        /// </summary>
        public Guid SubjectId { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
    }
}

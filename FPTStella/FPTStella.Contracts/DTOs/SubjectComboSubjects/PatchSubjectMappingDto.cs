// File: FPTStella.Contracts/DTOs/SubjectComboSubjects/PatchSubjectMappingDto.cs
using System;
using System.Collections.Generic;

namespace FPTStella.Contracts.DTOs.SubjectComboSubjects
{
    public class PatchSubjectMappingDto
    {
        /// <summary>
        /// The ID of the subject
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// List of subject combo IDs to associate with the subject
        /// </summary>
        public List<Guid> SubjectComboIds { get; set; } = new List<Guid>();
    }
}
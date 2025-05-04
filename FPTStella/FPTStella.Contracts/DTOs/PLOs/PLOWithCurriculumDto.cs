using System;

namespace FPTStella.Contracts.DTOs.PLOs
{
    /// <summary>
    /// Data Transfer Object for Program Learning Outcomes with curriculum information
    /// </summary>
    public class PLOWithCurriculumDto
    {
        /// <summary>
        /// The unique identifier of the PLO
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The name of the PLO
        /// </summary>
        public string PloName { get; set; } = string.Empty;

        /// <summary>
        /// The name of the curriculum this PLO belongs to
        /// </summary>
        public string CurriculumName { get; set; } = string.Empty;

        /// <summary>
        /// The description of the PLO
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The ID of the curriculum this PLO belongs to
        /// </summary>
        public Guid CurriculumId { get; set; }
    }
}
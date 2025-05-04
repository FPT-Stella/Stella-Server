using System;

namespace FPTStella.Contracts.DTOs.CLOs
{
    /// <summary>
    /// Data Transfer Object for Course Learning Outcomes with details
    /// </summary>
    public class CLOWithDetailsDto
    {
        /// <summary>
        /// The unique identifier of the CLO
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// The subject ID that this CLO belongs to
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// The name of the CLO
        /// </summary>
        public string CloName { get; set; } = string.Empty;

        /// <summary>
        /// The detailed description of the CLO
        /// </summary>
        public string CloDetails { get; set; } = string.Empty;

        /// <summary>
        /// Learning outcome details
        /// </summary>
        public string LoDetails { get; set; } = string.Empty;
    }
}
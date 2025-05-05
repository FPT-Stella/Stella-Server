using System.ComponentModel.DataAnnotations;

namespace FPTStella.Contracts.DTOs.CLOs
{
    /// <summary>
    /// Data Transfer Object for updating Course Learning Outcomes
    /// </summary>
    public class UpdateCLODto
    {
        /// <summary>
        /// The name of the CLO
        /// </summary>
       
        public string? CloName { get; set; }

        /// <summary>
        /// The updated details of the CLO
        /// </summary>
       
        public string? CloDetails { get; set; }

        /// <summary>
        /// Learning outcome details
        /// </summary>
    
        public string? LoDetails { get; set; }
    }
}
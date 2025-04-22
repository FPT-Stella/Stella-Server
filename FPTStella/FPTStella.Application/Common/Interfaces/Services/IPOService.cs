using FPTStella.Contracts.DTOs.POs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IPOService
    {
        Task<POsDto> CreatePOAsync(CreatePOsDto createPOsDto);
        Task<List<POsDto>> GetAllPOsAsync();
        Task<POsDto> GetPOByIdAsync(Guid id);
        Task<List<POsDto>> GetPOsByProgramIdAsync(Guid programId);
        /// <summary>
        /// Get POs by a list of program IDs.
        /// </summary>
        /// <param name="programIds"></param>
        /// <returns></returns>
        Task<List<POsDto>> GetPOsByProgramIdsAsync(List<Guid> programIds); 
        Task<bool> IsPoNameExistedAsync(Guid programId, string poName);
        Task UpdatePOAsync(Guid id, UpdatePOsDto updatePOsDto);
        Task DeletePOAsync(Guid id);
        /// <summary>
        /// Delete POs by program ID. This method sets the DelFlg to true and updates the UpdDate field.
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        Task DeletePOsByProgramIdAsync(Guid programId); 
    }
}

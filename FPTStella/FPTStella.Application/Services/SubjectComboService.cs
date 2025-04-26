using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.SubjectCombos;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class SubjectComboService : ISubjectComboService
    {
       private readonly IUnitOfWork _unitOfWork;
        private readonly ISubjectComboRepository _subjectComboRepository;
        public SubjectComboService(IUnitOfWork unitOfWork, ISubjectComboRepository subjectComboRepository)
        {
            _unitOfWork = unitOfWork;
            _subjectComboRepository = subjectComboRepository;
        }
        private static SubjectComboDto MapToDto(SubjectCombo entity)
        {
            return new SubjectComboDto
            {
                Id = entity.Id,
                ProgramId = entity.ProgramId,
                ComboName = entity.ComboName,
                Description = entity.Description,
                ProgramOutcome = entity.ProgramOutcome
            };
        }
        /// <summary>
        /// Creates a new subject combo
        /// </summary>
        public async Task<SubjectComboDto> CreateComboAsync(CreateSubjectComboDto createDto)
        {
            if (await _subjectComboRepository.IsComboNameExistedInProgramAsync(createDto.ProgramId, createDto.ComboName))
            {
                throw new InvalidOperationException($"A subject combo with name '{createDto.ComboName}' already exists in this program.");
            }

            var combo = new SubjectCombo
            {
                ProgramId = createDto.ProgramId,
                ComboName = createDto.ComboName,
                Description = createDto.Description,
                ProgramOutcome = createDto.ProgramOutcome,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _subjectComboRepository.InsertAsync(combo);
            await _unitOfWork.SaveAsync();

            return MapToDto(combo);
        }

        /// <summary>
        /// Gets a subject combo by its ID
        /// </summary>
        public async Task<SubjectComboDto> GetComboByIdAsync(Guid id)
        {
            var combo = await _subjectComboRepository.GetByIdAsync(id.ToString());

            if (combo == null || combo.DelFlg)
            {
                throw new KeyNotFoundException($"Subject combo with ID {id} not found.");
            }

            return MapToDto(combo);
        }

        /// <summary>
        /// Gets a subject combo by its name
        /// </summary>
        public async Task<SubjectComboDto> GetComboByNameAsync(string comboName)
        {
            var combo = await _subjectComboRepository.GetByComboNameAsync(comboName);

            if (combo == null || combo.DelFlg)
            {
                throw new KeyNotFoundException($"Subject combo with name '{comboName}' not found.");
            }

            return MapToDto(combo);
        }

        /// <summary>
        /// Gets all subject combos for a specific program
        /// </summary>
        public async Task<List<SubjectComboDto>> GetCombosByProgramIdAsync(Guid programId)
        {
            var combos = await _subjectComboRepository.GetByProgramIdAsync(programId);
            return combos.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Updates a subject combo
        /// </summary>
        public async Task<bool> UpdateComboAsync(Guid id, UpdateSubjectComboDto updateDto)
        {
            var combo = await _subjectComboRepository.GetByIdAsync(id.ToString());

            if (combo == null || combo.DelFlg)
            {
                throw new KeyNotFoundException($"Subject combo with ID {id} not found.");
            }

            // Check if combo name is changed and if it already exists in the program
            if (updateDto.ComboName != null &&
                updateDto.ComboName != combo.ComboName &&
                await _subjectComboRepository.IsComboNameExistedInProgramAsync(combo.ProgramId, updateDto.ComboName))
            {
                throw new InvalidOperationException($"A subject combo with name '{updateDto.ComboName}' already exists in this program.");
            }

            // Update properties if provided
            if (updateDto.ComboName != null)
                combo.ComboName = updateDto.ComboName;

            if (updateDto.Description != null)
                combo.Description = updateDto.Description;

            if (updateDto.ProgramOutcome != null)
                combo.ProgramOutcome = updateDto.ProgramOutcome;

            combo.UpdDate = DateTime.UtcNow;

            await _subjectComboRepository.ReplaceAsync(id.ToString(), combo);
            await _unitOfWork.SaveAsync();

            return true;
        }

        /// <summary>
        /// Deletes a subject combo
        /// </summary>
        public async Task<bool> DeleteComboAsync(Guid id)
        {
            try
            {
                await _subjectComboRepository.DeleteAsync(id.ToString());
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete subject combo: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes all subject combos for a specific program
        /// </summary>
        public async Task<bool> DeleteCombosByProgramIdAsync(Guid programId)
        {
            try
            {
                await _subjectComboRepository.DeleteByProgramIdAsync(programId);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete subject combos for program: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Searches for subject combos with pagination
        /// </summary>
        public async Task<PagedResult<SubjectComboDto>> SearchCombosAsync(
            string searchTerm,
            Guid? programId,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _subjectComboRepository.SearchComboAsync(searchTerm, programId, paginationParams);

            return new PagedResult<SubjectComboDto>
            {
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result.Items.Select(MapToDto)
            };
        }

        /// <summary>
        /// Checks if a combo name already exists within a program
        /// </summary>
        public async Task<bool> IsComboNameExistedAsync(Guid programId, string comboName)
        {
            return await _subjectComboRepository.IsComboNameExistedInProgramAsync(programId, comboName);
        }
    }
}

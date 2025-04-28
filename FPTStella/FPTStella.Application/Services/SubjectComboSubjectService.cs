using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.SubjectComboSubjects;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class SubjectComboSubjectService : ISubjectComboSubjectService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubjectComboSubjectRepository _subjectComboSubjectRepository;
        public SubjectComboSubjectService(IUnitOfWork unitOfWork, ISubjectComboSubjectRepository subjectComboSubjectRepository )
        {
            _unitOfWork = unitOfWork;
            _subjectComboSubjectRepository = subjectComboSubjectRepository;
        }
        /// <summary>
        /// Maps a SubjectComboSubjects entity to its DTO representation
        /// </summary>
        private static SubjectComboSubjectDto MapToDto(SubjectComboSubjects entity)
        {
            return new SubjectComboSubjectDto
            {
                Id = entity.Id,
                SubjectComboId = entity.SubjectComboId,
                SubjectId = entity.SubjectId
            };
        }

        /// <summary>
        /// Creates a new mapping between a subject combo and a subject
        /// </summary>
        public async Task<SubjectComboSubjectDto> CreateMappingAsync(CreateSubjectComboSubjectDto createDto)
        {
            // Check if mapping already exists
            if (await _subjectComboSubjectRepository.IsMappingExistedAsync(createDto.SubjectComboId, createDto.SubjectId))
            {
                throw new InvalidOperationException($"A mapping between subject combo ID {createDto.SubjectComboId} and subject ID {createDto.SubjectId} already exists.");
            }

            var mapping = new SubjectComboSubjects
            {
                SubjectComboId = createDto.SubjectComboId,
                SubjectId = createDto.SubjectId,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _subjectComboSubjectRepository.InsertAsync(mapping);
            await _unitOfWork.SaveAsync();

            return MapToDto(mapping);
        }

        /// <summary>
        /// Gets a mapping by its ID
        /// </summary>
        public async Task<SubjectComboSubjectDto> GetMappingByIdAsync(Guid id)
        {
            var mapping = await _subjectComboSubjectRepository.GetByIdAsync(id.ToString());

            if (mapping == null || mapping.DelFlg)
            {
                throw new KeyNotFoundException($"Subject combo subject mapping with ID {id} not found.");
            }

            return MapToDto(mapping);
        }

        /// <summary>
        /// Gets all mappings for a specific subject combo
        /// </summary>
        public async Task<List<SubjectComboSubjectDto>> GetMappingsBySubjectComboIdAsync(Guid subjectComboId)
        {
            var mappings = await _subjectComboSubjectRepository.GetBySubjectComboIdAsync(subjectComboId);
            return mappings.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Gets all mappings for a specific subject
        /// </summary>
        public async Task<List<SubjectComboSubjectDto>> GetMappingsBySubjectIdAsync(Guid subjectId)
        {
            var mappings = await _subjectComboSubjectRepository.GetBySubjectIdAsync(subjectId);
            return mappings.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Gets all mappings
        /// </summary>
        public async Task<List<SubjectComboSubjectDto>> GetAllMappingsAsync()
        {
            var mappings = await _subjectComboSubjectRepository.GetAllAsync();
            return mappings.Select(MapToDto).ToList();
        }

        /// <summary>
        /// Deletes a mapping by its ID
        /// </summary>
        public async Task<bool> DeleteMappingAsync(Guid id)
        {
            try
            {
                await _subjectComboSubjectRepository.DeleteAsync(id.ToString());
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete mapping: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes all mappings for a specific subject combo
        /// </summary>
        public async Task<bool> DeleteMappingsBySubjectComboIdAsync(Guid subjectComboId)
        {
            try
            {
                await _subjectComboSubjectRepository.DeleteMappingsBySubjectComboIdAsync(subjectComboId);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete mappings for subject combo: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Deletes all mappings for a specific subject
        /// </summary>
        public async Task<bool> DeleteMappingsBySubjectIdAsync(Guid subjectId)
        {
            try
            {
                await _subjectComboSubjectRepository.DeleteMappingsBySubjectIdAsync(subjectId);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete mappings for subject: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Gets all subject IDs associated with a specific subject combo
        /// </summary>
        public async Task<List<Guid>> GetSubjectIdsBySubjectComboIdAsync(Guid subjectComboId)
        {
            return await _subjectComboSubjectRepository.GetSubjectIdsBySubjectComboIdAsync(subjectComboId);
        }

        /// <summary>
        /// Gets all subject combo IDs that contain a specific subject
        /// </summary>
        public async Task<List<Guid>> GetSubjectComboIdsBySubjectIdAsync(Guid subjectId)
        {
            return await _subjectComboSubjectRepository.GetSubjectComboIdsBySubjectIdAsync(subjectId);
        }

        /// <summary>
        /// Checks if a mapping between a subject combo and a subject already exists
        /// </summary>
        public async Task<bool> IsMappingExistedAsync(Guid subjectComboId, Guid subjectId)
        {
            return await _subjectComboSubjectRepository.IsMappingExistedAsync(subjectComboId, subjectId);
        }

        /// <summary>
        /// Searches for subject combo subject mappings with pagination
        /// </summary>
        public async Task<PagedResult<SubjectComboSubjectDto>> SearchMappingsAsync(
            Guid? subjectComboId = null,
            Guid? subjectId = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _subjectComboSubjectRepository.SearchMappingsAsync(subjectComboId, subjectId, paginationParams);

            return new PagedResult<SubjectComboSubjectDto>
            {
                CurrentPage = result.CurrentPage,
                PageSize = result.PageSize,
                TotalCount = result.TotalCount,
                TotalPages = result.TotalPages,
                Items = result.Items.Select(MapToDto)
            };
        }
    }
}

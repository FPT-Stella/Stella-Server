using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.SubjectInCurriculums;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class SubjectInCurriculumService : ISubjectInCurriculumService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISubjectInCurriculumRepository _subjectInCurriculumRepository;
        private readonly ISubjectRepository _subjectRepository;
        /// Initializes a new instance of the SubjectInCurriculumService class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work</param>
        /// <param name="subjectInCurriculumRepository">The subject in curriculum repository</param>
        /// <param name="subjectRepository">The subject repository</param>
        public SubjectInCurriculumService(
            IUnitOfWork unitOfWork,
            ISubjectInCurriculumRepository subjectInCurriculumRepository,
            ISubjectRepository subjectRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _subjectInCurriculumRepository = subjectInCurriculumRepository ?? throw new ArgumentNullException(nameof(subjectInCurriculumRepository));
            _subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));
        }

        /// <summary>
        /// Maps a SubjectInCurriculum entity to its DTO representation.
        /// </summary>
        /// <param name="entity">The SubjectInCurriculum entity</param>
        /// <returns>A mapped SubjectInCurriculumDto object</returns>
        private async Task<SubjectInCurriculumDto> MapToDtoAsync(SubjectInCurriculum entity)
        {
            var dto = new SubjectInCurriculumDto
            {
                Id = entity.Id,
                CurriculumId = entity.CurriculumId,
                SubjectId = entity.SubjectId
            };

            // Fetch the subject details
            var subject = await _subjectRepository.GetByIdAsync(entity.SubjectId.ToString());
            if (subject != null && !subject.DelFlg)
            {
                dto.SubjectCode = subject.SubjectCode;
                dto.SubjectName = subject.SubjectName;
            }

            return dto;
        }

        /// <summary>
        /// Creates a new mapping between a subject and a curriculum.
        /// </summary>
        /// <param name="createDto">Data for creating the mapping</param>
        /// <returns>The created mapping as a DTO</returns>
        public async Task<SubjectInCurriculumDto> CreateMappingAsync(CreateSubjectInCurriculumDto createDto)
        {
            if (await _subjectInCurriculumRepository.IsMappingExistedAsync(createDto.SubjectId, createDto.CurriculumId))
            {
                throw new InvalidOperationException($"A mapping between subject ID {createDto.SubjectId} and curriculum ID {createDto.CurriculumId} already exists.");
            }

            var mapping = new SubjectInCurriculum
            {
                CurriculumId = createDto.CurriculumId,
                SubjectId = createDto.SubjectId,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _subjectInCurriculumRepository.InsertAsync(mapping);
            await _unitOfWork.SaveAsync();

            return await MapToDtoAsync(mapping);
        }

        /// <summary>
        /// Updates an existing subject-curriculum mapping.
        /// </summary>
        /// <param name="id">ID of the mapping to update</param>
        /// <param name="updateDto">Updated mapping data</param>
        public async Task UpdateMappingAsync(Guid id, UpdateSubjectInCurriculumDto updateDto)
        {
            var mapping = await _subjectInCurriculumRepository.GetByIdAsync(id.ToString());

            if (mapping == null || mapping.DelFlg)
            {
                throw new KeyNotFoundException($"Subject-Curriculum mapping with ID {id} not found.");
            }

            bool needToCheckForExisting = false;
            Guid newSubjectId = mapping.SubjectId;
            Guid newCurriculumId = mapping.CurriculumId;
            if (updateDto.SubjectId.HasValue)
            {
                if (updateDto.SubjectId.Value != mapping.SubjectId)
                {
                    newSubjectId = updateDto.SubjectId.Value;
                    needToCheckForExisting = true;
                }
            }

            if (updateDto.CurriculumId.HasValue)
            {
                if (updateDto.CurriculumId.Value != mapping.CurriculumId)
                {
                    newCurriculumId = updateDto.CurriculumId.Value;
                    needToCheckForExisting = true;
                }
            }
            if (needToCheckForExisting && await _subjectInCurriculumRepository.IsMappingExistedAsync(newSubjectId, newCurriculumId))
            {
                throw new InvalidOperationException($"A mapping between subject ID {newSubjectId} and curriculum ID {newCurriculumId} already exists.");
            }
            mapping.SubjectId = newSubjectId;
            mapping.CurriculumId = newCurriculumId;
            mapping.UpdDate = DateTime.UtcNow;

            await _subjectInCurriculumRepository.ReplaceAsync(id.ToString(), mapping);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Gets a specific subject-curriculum mapping by its ID.
        /// </summary>
        /// <param name="id">The ID of the mapping</param>
        /// <returns>The mapping as a DTO</returns>
        public async Task<SubjectInCurriculumDto> GetMappingByIdAsync(Guid id)
        {
            var mapping = await _subjectInCurriculumRepository.GetByIdAsync(id.ToString());

            if (mapping == null || mapping.DelFlg)
            {
                throw new KeyNotFoundException($"Subject-Curriculum mapping with ID {id} not found.");
            }

            return await MapToDtoAsync(mapping);
        }

        /// <summary>
        /// Gets all active subject-curriculum mappings.
        /// </summary>
        /// <returns>List of all active mappings</returns>
        public async Task<List<SubjectInCurriculumDto>> GetAllMappingsAsync()
        {
            var mappings = await _subjectInCurriculumRepository.FilterByAsync(m => !m.DelFlg);
            var result = new List<SubjectInCurriculumDto>();

            foreach (var mapping in mappings)
            {
                result.Add(await MapToDtoAsync(mapping));
            }

            return result;
        }

        /// <summary>
        /// Gets all mappings for a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>List of mappings for the curriculum</returns>
        public async Task<List<SubjectInCurriculumDto>> GetMappingsByCurriculumIdAsync(Guid curriculumId)
        {
            var mappings = await _subjectInCurriculumRepository.GetByCurriculumIdAsync(curriculumId);
            var result = new List<SubjectInCurriculumDto>();

            foreach (var mapping in mappings)
            {
                result.Add(await MapToDtoAsync(mapping));
            }

            return result;
        }

        /// <summary>
        /// Gets all mappings for a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of mappings for the subject</returns>
        public async Task<List<SubjectInCurriculumDto>> GetMappingsBySubjectIdAsync(Guid subjectId)
        {
            var mappings = await _subjectInCurriculumRepository.GetBySubjectIdAsync(subjectId);
            var result = new List<SubjectInCurriculumDto>();

            foreach (var mapping in mappings)
            {
                result.Add(await MapToDtoAsync(mapping));
            }

            return result;
        }

        /// <summary>
        /// Gets subject IDs that are mapped to a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>List of subject IDs</returns>
        public async Task<List<Guid>> GetSubjectIdsByCurriculumIdAsync(Guid curriculumId)
        {
            return await _subjectInCurriculumRepository.GetSubjectIdsByCurriculumIdAsync(curriculumId);
        }

        /// <summary>
        /// Gets curriculum IDs that are mapped to a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <returns>List of curriculum IDs</returns>
        public async Task<List<Guid>> GetCurriculumIdsBySubjectIdAsync(Guid subjectId)
        {
            return await _subjectInCurriculumRepository.GetCurriculumIdsBySubjectIdAsync(subjectId);
        }

        /// <summary>
        /// Checks if a mapping between a subject and curriculum exists.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        /// <param name="curriculumId">The curriculum ID</param>
        /// <returns>True if mapping exists, otherwise false</returns>
        public async Task<bool> IsMappingExistedAsync(Guid subjectId, Guid curriculumId)
        {
            return await _subjectInCurriculumRepository.IsMappingExistedAsync(subjectId, curriculumId);
        }

        /// <summary>
        /// Deletes a mapping by its ID.
        /// </summary>
        /// <param name="id">The ID of the mapping to delete</param>
        public async Task DeleteMappingAsync(Guid id)
        {
            var mapping = await _subjectInCurriculumRepository.GetByIdAsync(id.ToString());

            if (mapping == null || mapping.DelFlg)
            {
                throw new KeyNotFoundException($"Subject-Curriculum mapping with ID {id} not found.");
            }

            mapping.DelFlg = true;
            mapping.UpdDate = DateTime.UtcNow;

            await _subjectInCurriculumRepository.ReplaceAsync(id.ToString(), mapping);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Deletes all mappings for a specific subject.
        /// </summary>
        /// <param name="subjectId">The subject ID</param>
        public async Task DeleteMappingsBySubjectIdAsync(Guid subjectId)
        {
            await _subjectInCurriculumRepository.DeleteMappingsBySubjectIdAsync(subjectId);
            await _unitOfWork.SaveAsync();
        }

        /// <summary>
        /// Deletes all mappings for a specific curriculum.
        /// </summary>
        /// <param name="curriculumId">The curriculum ID</param>
        public async Task DeleteMappingsByCurriculumIdAsync(Guid curriculumId)
        {
            await _subjectInCurriculumRepository.DeleteMappingsByCurriculumIdAsync(curriculumId);
            await _unitOfWork.SaveAsync();
        }
    }
}

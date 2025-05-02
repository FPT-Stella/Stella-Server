using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.Materials;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class MaterialService : IMaterialService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMaterialRepository _materialRepository;
        public MaterialService(IUnitOfWork unitOfWork, IMaterialRepository materialRepository)
        {
            _unitOfWork = unitOfWork;
            _materialRepository = materialRepository;
        }
        private static MaterialDto MapToDto(Materials material)
        {
            return new MaterialDto
            {
                Id = material.Id.ToString(),
                SubjectId = material.SubjectId.ToString(),
                MaterialName = material.MaterialName,
                MaterialType = material.MaterialType,
                MaterialUrl = material.MaterialUrl,
                Description = material.Description,
                CreatedAt = material.InsDate,
                UpdatedAt = material.UpdDate,
                DelFlg = material.DelFlg
            };
        }
        /// <summary>
        /// Creates a new material
        /// </summary>
        public async Task<MaterialDto> CreateMaterialAsync(CreateMaterialDto createMaterialDto)
        {
            // Check if material name already exists
            var existingMaterial = await _materialRepository.GetByMaterialNameAsync(createMaterialDto.MaterialName);
            if (existingMaterial != null)
            {
                throw new InvalidOperationException($"Material with name '{createMaterialDto.MaterialName}' already exists.");
            }

            // Validate subject exists
            if (!Guid.TryParse(createMaterialDto.SubjectId, out var subjectId))
            {
                throw new ArgumentException("Invalid SubjectId format.");
            }

            var subjectRepository = _unitOfWork.Repository<Subjects>();
            var subject = await subjectRepository.GetByIdAsync(subjectId.ToString());
            if (subject == null)
            {
                throw new KeyNotFoundException($"Subject with ID {subjectId} not found.");
            }

            // Create new material entity
            var material = new Materials
            {
                SubjectId = subjectId,
                MaterialName = createMaterialDto.MaterialName,
                MaterialType = createMaterialDto.MaterialType,
                MaterialUrl = createMaterialDto.MaterialUrl,
                Description = createMaterialDto.Description,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _materialRepository.InsertAsync(material);
            await _unitOfWork.SaveAsync();

            return MapToDto(material);
        }
        /// <summary>
        /// Gets a material by its ID
        /// </summary>
        public async Task<MaterialDto> GetMaterialByIdAsync(string id)
        {
            var material = await _materialRepository.GetByIdAsync(id);

            if (material == null || material.DelFlg)
            {
                throw new KeyNotFoundException($"Material with ID {id} not found.");
            }
            return MapToDto(material);
        }
        /// <summary>
        /// Gets a material by its name
        /// </summary>
        public async Task<MaterialDto?> GetMaterialByNameAsync(string materialName)
        {
            var material = await _materialRepository.GetByMaterialNameAsync(materialName);

            if (material == null || material.DelFlg)
            {
                return null;
            }

            return MapToDto(material);
        }
        /// <summary>
        /// Gets all materials for a specific subject
        /// </summary>
        public async Task<List<MaterialDto>> GetMaterialsBySubjectIdAsync(Guid subjectId)
        {
            var materials = await _materialRepository.GetBySubjectIdAsync(subjectId);
            return materials.Select(MapToDto).ToList();
        }
        /// <summary>
        /// Gets all materials with a specific type
        /// </summary>
        public async Task<List<MaterialDto>> GetMaterialsByTypeAsync(string materialType)
        {
            var materials = await _materialRepository.GetByMaterialTypeAsync(materialType);
            return materials.Select(MapToDto).ToList();
        }
        /// <summary>
        /// Gets all materials
        /// </summary>
        public async Task<List<MaterialDto>> GetAllMaterialsAsync()
        {
            var materials = await _materialRepository.GetAllAsync();
            return materials.Where(m => !m.DelFlg).Select(MapToDto).ToList();
        }
        /// <summary>
        /// Updates a material
        /// </summary>
        public async Task<bool> UpdateMaterialAsync(string id, UpdateMaterialDto updateMaterialDto)
        {
            var material = await _materialRepository.GetByIdAsync(id);

            if (material == null || material.DelFlg)
            {
                throw new KeyNotFoundException($"Material with ID {id} not found.");
            }

            if (!string.IsNullOrEmpty(updateMaterialDto.MaterialName) &&
                material.MaterialName != updateMaterialDto.MaterialName)
            {
                var existingMaterial = await _materialRepository.GetByMaterialNameAsync(updateMaterialDto.MaterialName);
                if (existingMaterial != null && existingMaterial.Id != material.Id)
                {
                    throw new InvalidOperationException($"Material with name '{updateMaterialDto.MaterialName}' already exists.");
                }
            }
            material.MaterialName = !string.IsNullOrEmpty(updateMaterialDto.MaterialName)
                ? updateMaterialDto.MaterialName
                : material.MaterialName;
            material.MaterialType = !string.IsNullOrEmpty(updateMaterialDto.MaterialType)
                ? updateMaterialDto.MaterialType
                : material.MaterialType;
            material.MaterialUrl = !string.IsNullOrEmpty(updateMaterialDto.MaterialUrl)
                ? updateMaterialDto.MaterialUrl
                : material.MaterialUrl;
            material.Description = updateMaterialDto.Description ?? material.Description;
            material.UpdDate = DateTime.UtcNow;

            await _materialRepository.ReplaceAsync(id, material);
            await _unitOfWork.SaveAsync();

            return true;
        }
        /// <summary>
        /// Deletes a material by its ID
        /// </summary>
        public async Task<bool> DeleteMaterialAsync(string id)
        {
            try
            {
                await _materialRepository.DeleteAsync(id);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete material: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Deletes all materials for a specific subject
        /// </summary>
        public async Task<bool> DeleteMaterialsBySubjectIdAsync(Guid subjectId)
        {
            try
            {
                var materials = await _materialRepository.GetBySubjectIdAsync(subjectId);

                foreach (var material in materials)
                {
                    material.DelFlg = true;
                    material.UpdDate = DateTime.UtcNow;
                    await _materialRepository.ReplaceAsync(material.Id.ToString(), material);
                }

                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete materials for subject: {ex.Message}", ex);
            }
        }
        /// <summary>
        /// Searches for materials with pagination
        /// </summary>
        public async Task<PagedResult<MaterialDto>> SearchMaterialsAsync(
            string? searchTerm = null,
            Guid? subjectId = null,
            string? materialType = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            var result = await _materialRepository.SearchMaterialsAsync(
                searchTerm,
                subjectId,
                materialType,
                paginationParams);

            return new PagedResult<MaterialDto>
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

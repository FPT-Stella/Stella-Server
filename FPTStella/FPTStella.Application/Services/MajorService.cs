using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.Majors;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class MajorService : IMajorService
    {
        private readonly IUnitOfWork _unitOfWork;
        public MajorService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private static MajorDto MapToMajorDto(Majors major)
        {
            return new MajorDto
            {
                Id = major.Id.ToString(),
                MajorName = major.MajorName,
                Description = major.Description,
            };
        }
        public async Task<MajorDto> CreateMajorAsync(CreateMajorDto createMajorDto)
        {
            var majorRepository = _unitOfWork.Repository<Majors>();
            if (string.IsNullOrWhiteSpace(createMajorDto.MajorName))
            {
                throw new ArgumentException("Major name cannot be null or empty.");
            }

            var existingMajor = await majorRepository.FindOneAsync(m => m.MajorName == createMajorDto.MajorName);
            if (existingMajor != null)
            {
                throw new InvalidOperationException("Major with this name already exists.");
            }
            var major = new Majors
            {
                MajorName = createMajorDto.MajorName,
                Description = createMajorDto.Description,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };
            await majorRepository.InsertAsync(major);
            await _unitOfWork.SaveAsync();
            return MapToMajorDto(major);
        }
        public async Task<List<MajorDto>> GetAllMajorsAsync()
        {
            var majorRepository = _unitOfWork.Repository<Majors>();
            var majors = await majorRepository.GetAllAsync();
            return majors.Select(MapToMajorDto).ToList();
        }
        public async Task<MajorDto> GetMajorByIdAsync(Guid id)
        {
            var majorRepository = _unitOfWork.Repository<Majors>();
            var major = await majorRepository.GetByIdAsync(id.ToString());

            if (major == null || major.DelFlg)
            {
                throw new KeyNotFoundException("Major not found.");
            }

            return MapToMajorDto(major);
        }
        public async Task UpdateMajorAsync(Guid id, UpdateMajorDto updateMajorDto)
        {
            var majorRepository = _unitOfWork.Repository<Majors>();
            var major = await majorRepository.GetByIdAsync(id.ToString());

            if (major == null || major.DelFlg)
            {
                throw new KeyNotFoundException("Major not found.");
            }

            if (!string.IsNullOrWhiteSpace(updateMajorDto.MajorName))
            {
                var existingMajor = await majorRepository.FindOneAsync(m => m.MajorName == updateMajorDto.MajorName && m.Id != id);
                if (existingMajor != null)
                {
                    throw new InvalidOperationException("Major with this name already exists.");
                }

                major.MajorName = updateMajorDto.MajorName;
            }

            if (!string.IsNullOrWhiteSpace(updateMajorDto.Description))
            {
                major.Description = updateMajorDto.Description;
            }

            major.UpdDate = DateTime.UtcNow;

            await majorRepository.ReplaceAsync(id.ToString(), major);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteMajorAsync(Guid id)
        {
            var majorRepository = _unitOfWork.Repository<Majors>();
            var major = await majorRepository.GetByIdAsync(id.ToString());

            if (major == null || major.DelFlg)
            {
                throw new KeyNotFoundException("Major not found.");
            }

            major.DelFlg = true;
            major.UpdDate = DateTime.UtcNow;

            await majorRepository.ReplaceAsync(id.ToString(), major);
            await _unitOfWork.SaveAsync();
        }
        public async Task<bool> MajorExistsAsync(string majorName)
        {
            var majorRepository = _unitOfWork.Repository<Majors>();
            var existingMajor = await majorRepository.FindOneAsync(m => m.MajorName == majorName && !m.DelFlg);
            return existingMajor != null;
        }

        public async Task<bool> MajorExistsByIdAsync(Guid id)
        {
            var majorRepository = _unitOfWork.Repository<Majors>();
            var existingMajor = await majorRepository.GetByIdAsync(id.ToString());
            return existingMajor != null && !existingMajor.DelFlg;
        }
    }
}

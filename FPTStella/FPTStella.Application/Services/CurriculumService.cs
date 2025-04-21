using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.Curriculums;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class CurriculumService : ICurriculumService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CurriculumService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private static CurriculumDto MapToCurriculumDto(Curriculums curriculum)
        {
            return new CurriculumDto
            {
                Id = curriculum.Id,
                ProgramId = curriculum.ProgramId,
                CurriculumCode = curriculum.CurriculumCode,
                CurriculumName = curriculum.CurriculumName,
                Description = curriculum.Description,
                TotalCredit = curriculum.TotalCredit,
                StartYear = curriculum.StartYear,
                EndYear = curriculum.EndYear
            };
        }
        public async Task<CurriculumDto> CreateCurriculumAsync(CreateCurriculumDto createCurriculumDto)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();

            if (await curriculumRepository.FindOneAsync(c => c.CurriculumCode == createCurriculumDto.CurriculumCode) != null)
            {
                throw new InvalidOperationException("CurriculumCode already exists.");
            }

            if (await curriculumRepository.FindOneAsync(c => c.CurriculumName == createCurriculumDto.CurriculumName) != null)
            {
                throw new InvalidOperationException("CurriculumName already exists.");
            }

            var curriculum = new Curriculums
            {
                ProgramId = createCurriculumDto.ProgramId,
                CurriculumCode = createCurriculumDto.CurriculumCode,
                CurriculumName = createCurriculumDto.CurriculumName,
                Description = createCurriculumDto.Description,
                TotalCredit = createCurriculumDto.TotalCredit,
                StartYear = createCurriculumDto.StartYear,
                EndYear = createCurriculumDto.EndYear,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await curriculumRepository.InsertAsync(curriculum);
            await _unitOfWork.SaveAsync();

            return MapToCurriculumDto(curriculum);
        }
        public async Task<List<CurriculumDto>> GetAllCurriculumsAsync()
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            var curriculums = await curriculumRepository.FilterByAsync(c => !c.DelFlg);

            return curriculums.Select(MapToCurriculumDto).ToList();
        }

        public async Task<CurriculumDto> GetCurriculumByIdAsync(Guid id)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            var curriculum = await curriculumRepository.GetByIdAsync(id.ToString());

            if (curriculum == null || curriculum.DelFlg)
            {
                throw new KeyNotFoundException("Curriculum not found.");
            }

            return MapToCurriculumDto(curriculum);
        }
        public async Task<CurriculumDto> GetCurriculumByCurriculumCodeAsync(string curriculumCode)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            var curriculum = await curriculumRepository.FindOneAsync(c => c.CurriculumCode == curriculumCode);

            if (curriculum == null)
            {
                throw new KeyNotFoundException("Curriculum not found.");
            }

            return MapToCurriculumDto(curriculum);
        }
        public async Task<CurriculumDto> GetCurriculumByCurriculumNameAsync(string curriculumName)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            var curriculum = await curriculumRepository.FindOneAsync(c => c.CurriculumName == curriculumName);

            if (curriculum == null)
            {
                throw new KeyNotFoundException("Curriculum not found.");
            }

            return MapToCurriculumDto(curriculum);
        }
        public async Task<List<CurriculumDto>> GetCurriculumsByProgramIdAsync(Guid programId)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            var curriculums = await curriculumRepository.FilterByAsync(c => c.ProgramId == programId && !c.DelFlg);

            return curriculums.Select(MapToCurriculumDto).ToList();
        }
        public async Task UpdateCurriculumAsync(Guid id, UpdateCurriculumDto updateCurriculumDto)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            var curriculum = await curriculumRepository.GetByIdAsync(id.ToString());

            if (curriculum == null || curriculum.DelFlg)
            {
                throw new KeyNotFoundException("Curriculum not found.");
            }

            if (!string.IsNullOrWhiteSpace(updateCurriculumDto.CurriculumCode) &&
                updateCurriculumDto.CurriculumCode != curriculum.CurriculumCode &&
                await curriculumRepository.FindOneAsync(c => c.CurriculumCode == updateCurriculumDto.CurriculumCode) != null)
            {
                throw new InvalidOperationException("CurriculumCode already exists.");
            }

            if (!string.IsNullOrWhiteSpace(updateCurriculumDto.CurriculumName) &&
                updateCurriculumDto.CurriculumName != curriculum.CurriculumName &&
                await curriculumRepository.FindOneAsync(c => c.CurriculumName == updateCurriculumDto.CurriculumName) != null)
            {
                throw new InvalidOperationException("CurriculumName already exists.");
            }

            curriculum.ProgramId = updateCurriculumDto.ProgramId ?? curriculum.ProgramId;
            curriculum.CurriculumCode = updateCurriculumDto.CurriculumCode ?? curriculum.CurriculumCode;
            curriculum.CurriculumName = updateCurriculumDto.CurriculumName ?? curriculum.CurriculumName;
            curriculum.Description = updateCurriculumDto.Description ?? curriculum.Description;
            curriculum.TotalCredit = updateCurriculumDto.TotalCredit ?? curriculum.TotalCredit;
            curriculum.StartYear = updateCurriculumDto.StartYear ?? curriculum.StartYear;
            curriculum.EndYear = updateCurriculumDto.EndYear ?? curriculum.EndYear;
            curriculum.UpdDate = DateTime.UtcNow;

            await curriculumRepository.ReplaceAsync(id.ToString(), curriculum);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteCurriculumAsync(Guid id)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            var curriculum = await curriculumRepository.GetByIdAsync(id.ToString());

            if (curriculum == null || curriculum.DelFlg)
            {
                throw new KeyNotFoundException("Curriculum not found.");
            }

            curriculum.DelFlg = true;
            curriculum.UpdDate = DateTime.UtcNow;

            await curriculumRepository.ReplaceAsync(id.ToString(), curriculum);
            await _unitOfWork.SaveAsync();
        }
        public async Task<bool> IsCurriculumCodeExistedAsync(string curriculumCode)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            return await curriculumRepository.FindOneAsync(c => c.CurriculumCode == curriculumCode) != null;
        }

        public async Task<bool> IsCurriculumNameExistedAsync(string curriculumName)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            return await curriculumRepository.FindOneAsync(c => c.CurriculumName == curriculumName) != null;
        }

        public async Task<bool> IsCurriculumCodeExistedAsync(string curriculumCode, Guid id)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            return await curriculumRepository.FindOneAsync(c => c.CurriculumCode == curriculumCode && c.Id != id) != null;
        }

        public async Task<bool> IsCurriculumNameExistedAsync(string curriculumName, Guid id)
        {
            var curriculumRepository = _unitOfWork.Repository<Curriculums>();
            return await curriculumRepository.FindOneAsync(c => c.CurriculumName == curriculumName && c.Id != id) != null;
        }
    }
}

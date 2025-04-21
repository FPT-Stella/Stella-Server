using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.Programs;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class ProgramService : IProgramService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProgramService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private static ProgramDto MapToProgramDto(Programs program)
        {
            return new ProgramDto
            {
                Id = program.Id.ToString(),
                MajorId = program.MajorId.ToString(),
                ProgramCode = program.ProgramCode,
                ProgramName = program.ProgramName,
                Description = program.Description,
            };
        }
        public async Task<ProgramDto> CreateProgramAsync(CreateProgramDto createProgramDto)
        {
            var programRepository = _unitOfWork.Repository<Programs>();

            if (!Guid.TryParse(createProgramDto.MajorId, out var majorId))
            {
                throw new ArgumentException("Invalid MajorId format.");
            }

            var existingProgramByCode = await programRepository.FindOneAsync(p => p.ProgramCode == createProgramDto.ProgramCode);
            if (existingProgramByCode != null)
            {
                throw new InvalidOperationException("ProgramCode already exists.");
            }

            var existingProgramByName = await programRepository.FindOneAsync(p => p.ProgramName == createProgramDto.ProgramName);
            if (existingProgramByName != null)
            {
                throw new InvalidOperationException("ProgramName already exists.");
            }

            var program = new Programs
            {
                MajorId = majorId,
                ProgramCode = createProgramDto.ProgramCode,
                ProgramName = createProgramDto.ProgramName,
                Description = createProgramDto.Description,
            };
            await programRepository.InsertAsync(program);
            await _unitOfWork.SaveAsync();

            return MapToProgramDto(program);
        }
        public async Task<List<ProgramDto>> GetAllProgramsAsync()
        {
            var programRepository = _unitOfWork.Repository<Programs>();
            var programs = await programRepository.FilterByAsync(p => !p.DelFlg);

            return programs.Select(MapToProgramDto).ToList();
        }
        public async Task<ProgramDto> GetProgramByIdAsync(string id)
        {
            var programRepository = _unitOfWork.Repository<Programs>();
            var program = await programRepository.GetByIdAsync(id);

            if (program == null)
            {
                throw new KeyNotFoundException("Program not found.");
            }

            return MapToProgramDto(program);
        }
        public async Task<ProgramDto> GetProgramByProgramCodeAsync(string programCode)
        {
            var programRepository = _unitOfWork.Repository<Programs>();
            var program = await programRepository.FindOneAsync(p => p.ProgramCode == programCode);

            if (program == null)
            {
                throw new KeyNotFoundException("Program not found.");
            }

            return MapToProgramDto(program);
        }
        public async Task<ProgramDto> GetProgramByMajorIdAsync(Guid majorId)
        {
            var programRepository = _unitOfWork.Repository<Programs>();
            var program = await programRepository.FindOneAsync(p => p.MajorId == majorId);

            if (program == null)
            {
                throw new KeyNotFoundException("Program not found.");
            }

            return MapToProgramDto(program);
        }
        public async Task<ProgramDto> GetProgramByProgramNameAsync(string programName)
        {
            var programRepository = _unitOfWork.Repository<Programs>();
            var program = await programRepository.FindOneAsync(p => p.ProgramName == programName);

            if (program == null)
            {
                throw new KeyNotFoundException("Program not found.");
            }

            return MapToProgramDto(program);
        }
        public async Task<ProgramDto> GetProgramByMajorIdAndProgramNameAsync(Guid majorId, string programName)
        {
            var programRepository = _unitOfWork.Repository<Programs>();
            var program = await programRepository.FindOneAsync(p => p.MajorId == majorId && p.ProgramName == programName);

            if (program == null)
            {
                throw new KeyNotFoundException("Program not found.");
            }

            return MapToProgramDto(program);
        }
        public async Task<ProgramDto> GetProgramByMajorIdAndProgramCodeAsync(Guid majorId, string programCode)
        {
            var programRepository = _unitOfWork.Repository<Programs>();
            var program = await programRepository.FindOneAsync(p => p.MajorId == majorId && p.ProgramCode == programCode);

            if (program == null)
            {
                throw new KeyNotFoundException("Program not found.");
            }

            return MapToProgramDto(program);
        }
        public async Task UpdateProgramAsync(string id, UpdateProgramDto updateProgramDto)
        {
            var programRepository = _unitOfWork.Repository<Programs>();
            var program = await programRepository.GetByIdAsync(id);

            if (program == null)
            {
                throw new KeyNotFoundException("Program not found.");
            }

            if (program.ProgramCode != updateProgramDto.ProgramCode)
            {
                var existingProgramByCode = await programRepository.FindOneAsync(p => p.ProgramCode == updateProgramDto.ProgramCode);
                if (existingProgramByCode != null)
                {
                    throw new InvalidOperationException("ProgramCode already exists.");
                }
            }

            if (program.ProgramName != updateProgramDto.ProgramName)
            {
                var existingProgramByName = await programRepository.FindOneAsync(p => p.ProgramName == updateProgramDto.ProgramName);
                if (existingProgramByName != null)
                {
                    throw new InvalidOperationException("ProgramName already exists.");
                }
            }
            program.ProgramCode = updateProgramDto.ProgramCode;
            program.ProgramName = updateProgramDto.ProgramName;
            program.Description = updateProgramDto.Description;

            await programRepository.ReplaceAsync(id, program);
            await _unitOfWork.SaveAsync();
        }
        public async Task DeleteProgramAsync(string id)
        {
            var programRepository = _unitOfWork.Repository<Programs>();
            await programRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}

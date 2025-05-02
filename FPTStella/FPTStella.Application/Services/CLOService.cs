using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.CLOs;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class CLOService : ICLOService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICLORepository _cloRepository;

        public CLOService(IUnitOfWork unitOfWork, ICLORepository cloRepository)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _cloRepository = cloRepository ?? throw new ArgumentNullException(nameof(cloRepository));
        }

        public async Task<Guid> CreateCloAsync(CreateCLODto createCloDto)
        {
            if (createCloDto == null)
            {
                throw new ArgumentNullException(nameof(createCloDto));
            }

            // Check if subject exists
            var subjectRepository = _unitOfWork.Repository<Subjects>();
            var subject = await subjectRepository.GetByIdAsync(createCloDto.SubjectId.ToString());
            if (subject == null)
            {
                throw new KeyNotFoundException($"Subject with ID {createCloDto.SubjectId} not found");
            }

            // Check if CLO already exists for this subject
            if (await _cloRepository.IsCloExistedAsync(createCloDto.SubjectId, createCloDto.CloDetails))
            {
                throw new InvalidOperationException("A CLO with the same details already exists for this subject");
            }

            var clo = new CLOs
            {
                SubjectId = createCloDto.SubjectId,
                CloDetails = createCloDto.CloDetails,
                
            };
            await _cloRepository.InsertAsync(clo);
            await _unitOfWork.SaveAsync();

            return clo.Id;
        }

        public async Task<List<CLOWithDetailsDto>> GetClosBySubjectIdAsync(Guid subjectId)
        {
            var clos = await _cloRepository.GetBySubjectIdAsync(subjectId);

            return clos.Select(c => new CLOWithDetailsDto
            {
                Id = c.Id,
                Details = c.CloDetails
            }).ToList();
        }

        public async Task DeleteClosBySubjectIdAsync(Guid subjectId)
        {
            await _cloRepository.DeleteClosBySubjectIdAsync(subjectId);
            await _unitOfWork.SaveAsync();
        }
    }
}

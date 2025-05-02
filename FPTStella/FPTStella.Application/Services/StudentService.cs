using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Application.Utils;
using FPTStella.Contracts.DTOs.Students;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Services
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStudentRepository _studentRepository;

        public StudentService(IUnitOfWork unitOfWork, IStudentRepository studentRepository)
        {
            _unitOfWork = unitOfWork;
            _studentRepository = studentRepository;
        }
        private static StudentDto MapToStudentDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id.ToString(),
                UserId = student.UserId.ToString(),
                MajorId = student.MajorId.ToString(),
                StudentCode = student.StudentCode,
                Phone = student.Phone,
                Address = student.Address,
            };
        }
        public async Task<StudentDto> CreateStudentAsync(CreateStudentDto createStudentDto, HttpContext http)
        {
            var accountRepository = _unitOfWork.Repository<Account>();
            var majorRepository = _unitOfWork.Repository<Majors>();
            var accountId = UserUtil.GetAccountId(http);

            if (!Guid.TryParse(createStudentDto.MajorId, out var majorId))
            {
                throw new ArgumentException("Invalid MajorId format.");
            }

            var user = await accountRepository.GetByIdAsync(accountId.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (user.Role != FPTStella.Domain.Enums.Role.Student)
            {
                throw new InvalidOperationException("User must have role 'Student' to create a Student record.");
            }

            var studentByCode = await _studentRepository.GetByStudentCodeAsync(createStudentDto.StudentCode);
            if (studentByCode != null)
            {
                throw new InvalidOperationException("StudentCode already exists.");
            }

            var major = await majorRepository.GetByIdAsync(majorId.ToString());
            if (major == null)
            {
                throw new KeyNotFoundException("Major not found.");
            }

            var student = new Student
            {
                UserId = (Guid)accountId,
                MajorId = majorId,
                StudentCode = createStudentDto.StudentCode,
                Phone = createStudentDto.Phone,
                Address = createStudentDto.Address,
                InsDate = DateTime.UtcNow,
                UpdDate = DateTime.UtcNow,
                DelFlg = false
            };

            await _studentRepository.InsertAsync(student);
            await _unitOfWork.SaveAsync();

            return MapToStudentDto(student);
        }

        public async Task<StudentDto> GetStudentByIdAsync(string id)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            return MapToStudentDto(student);
        }

        public async Task<StudentDto> GetCurrentStudentAsync(HttpContext httpContext)
        {
            var accountId = UserUtil.GetAccountId(httpContext);
            if (accountId == null)
            {
                throw new UnauthorizedAccessException("User not authenticated.");
            }

            var student = await _studentRepository.GetByUserIdAsync((Guid)accountId);

            if (student == null)
            {
                throw new KeyNotFoundException("Student profile not found for current user.");
            }

            return MapToStudentDto(student);
        }

        public async Task<StudentDto> GetStudentByStudentCodeAsync(string studentCode)
        {
            var student = await _studentRepository.GetByStudentCodeAsync(studentCode);

            if (student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            return MapToStudentDto(student);
        }

        public async Task<StudentDto> GetStudentByUserIdAsync(string userId)
        {
            if (!Guid.TryParse(userId, out var guidUserId))
            {
                throw new ArgumentException("Invalid UserId format.");
            }

            var student = await _studentRepository.GetByUserIdAsync(guidUserId);

            if (student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            return MapToStudentDto(student);
        }

        public async Task UpdateStudentAsync(string id, UpdateStudentDto updateStudentDto)
        {
            var student = await _studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            if (student.StudentCode != updateStudentDto.StudentCode)
            {
                var studentByCode = await _studentRepository.GetByStudentCodeAsync(updateStudentDto.StudentCode);
                if (studentByCode != null)
                {
                    throw new InvalidOperationException("StudentCode already exists.");
                }
            }

            if (student.MajorId != updateStudentDto.MajorId)
            {
                var majorRepository = _unitOfWork.Repository<Majors>();
                var major = await majorRepository.GetByIdAsync(updateStudentDto.MajorId.ToString());
                if (major == null)
                {
                    throw new KeyNotFoundException("Major not found.");
                }
            }

            student.MajorId = updateStudentDto.MajorId;
            student.StudentCode = updateStudentDto.StudentCode;
            student.Phone = updateStudentDto.Phone;
            student.Address = updateStudentDto.Address;
            student.UpdDate = DateTime.UtcNow;

            await _studentRepository.ReplaceAsync(id, student);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteStudentAsync(string id)
        {
            await _studentRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }

        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            var students = await _studentRepository.GetAllAsync();
            return students.Select(MapToStudentDto).ToList();
        }

        /// <summary>
        /// Searches for students with advanced filtering options and pagination
        /// </summary>
        public async Task<PagedResult<StudentDto>> SearchStudentsAsync(
            string? searchTerm = null,
            Guid? majorId = null,
            int pageNumber = 1,
            int pageSize = 10)
        {
            var paginationParams = new PaginationParams
            {
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            var searchableFields = new[] { "StudentCode", "Phone", "Address" };
            var baseQuery = await _studentRepository.SearchAsync(
                searchTerm ?? string.Empty,
                paginationParams,
                searchableFields,
                false);
            var resultItems = baseQuery.Items;
            if (majorId.HasValue)
            {
                resultItems = resultItems.Where(s => s.MajorId == majorId.Value);
            }

            return new PagedResult<StudentDto>
            {
                CurrentPage = baseQuery.CurrentPage,
                PageSize = baseQuery.PageSize,
                TotalCount = baseQuery.TotalCount,
                TotalPages = baseQuery.TotalPages,
                Items = resultItems.Select(MapToStudentDto)
            };
        }
    }
}

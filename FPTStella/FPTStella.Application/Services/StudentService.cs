using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Contracts.DTOs.Students;
using FPTStella.Domain.Entities;
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

        public StudentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        private static StudentDto MapToStudentDto(Student student)
        {
            return new StudentDto
            {
                Id = student.Id.ToString(),
                UserId = student.UserId.ToString(),
                StudentCode = student.StudentCode,
                Phone = student.Phone,
                Address = student.Address,
            };
        }

        public async Task<StudentDto> CreateStudentAsync(CreateStudentDto createStudentDto)
        {
            var studentRepository = _unitOfWork.Repository<Student>();
            var accountRepository = _unitOfWork.Repository<Account>();

            if (!Guid.TryParse(createStudentDto.UserId, out var userId))
            {
                throw new ArgumentException("Invalid UserId format.");
            }

            var user = await accountRepository.GetByIdAsync(createStudentDto.UserId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found.");
            }

            if (user.Role != FPTStella.Domain.Enum.Role.Student)
            {
                throw new InvalidOperationException("User must have role 'Student' to create a Student record.");
            }

            var existingStudent = await studentRepository.FindOneAsync(s => s.UserId == userId);
            if (existingStudent != null)
            {
                throw new InvalidOperationException("A Student record already exists for this UserId.");
            }

            var studentByCode = await studentRepository.FindOneAsync(s => s.StudentCode == createStudentDto.StudentCode);
            if (studentByCode != null)
            {
                throw new InvalidOperationException("StudentCode already exists.");
            }

            var student = new Student
            {
                UserId = userId,
                StudentCode = createStudentDto.StudentCode,
                Phone = createStudentDto.Phone,
                Address = createStudentDto.Address,
            };

            await studentRepository.InsertAsync(student);
            await _unitOfWork.SaveAsync();

            return MapToStudentDto(student);
        }
        public async Task<StudentDto> GetStudentByIdAsync(string id)
        {
            var studentRepository = _unitOfWork.Repository<Student>();
            var student = await studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            return MapToStudentDto(student);
        }

        public async Task<StudentDto> GetStudentByStudentCodeAsync(string studentCode)
        {
            var studentRepository = _unitOfWork.Repository<Student>();
            var student = await studentRepository.FindOneAsync(s => s.StudentCode == studentCode);

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

            var studentRepository = _unitOfWork.Repository<Student>();
            var student = await studentRepository.FindOneAsync(s => s.UserId == guidUserId);

            if (student == null)
            {
                throw new KeyNotFoundException("Student not found for this UserId.");
            }

            return MapToStudentDto(student);
        }

        public async Task UpdateStudentAsync(string id, UpdateStudentDto updateStudentDto)
        {
            var studentRepository = _unitOfWork.Repository<Student>();
            var student = await studentRepository.GetByIdAsync(id);

            if (student == null)
            {
                throw new KeyNotFoundException("Student not found.");
            }

            if (student.StudentCode != updateStudentDto.StudentCode)
            {
                var studentByCode = await studentRepository.FindOneAsync(s => s.StudentCode == updateStudentDto.StudentCode);
                if (studentByCode != null)
                {
                    throw new InvalidOperationException("StudentCode already exists.");
                }
            }

            student.StudentCode = updateStudentDto.StudentCode;
            student.Phone = updateStudentDto.Phone;
            student.Address = updateStudentDto.Address;

            await studentRepository.ReplaceAsync(id, student);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteStudentAsync(string id)
        {
            var studentRepository = _unitOfWork.Repository<Student>();
            await studentRepository.DeleteAsync(id);
            await _unitOfWork.SaveAsync();
        }
    }
}

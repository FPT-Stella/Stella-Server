using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
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
        private readonly IStudentRepository _studentRepository;
        private readonly IUserRepository _userRepository;
        public StudentService(IStudentRepository studentRepository, IUserRepository userRepository)
        {
            _studentRepository = studentRepository;
            _userRepository = userRepository;
        }
        public async Task<StudentDto> CreateStudentAsync(CreateStudentDto createStudentDto)
        {
            if (!Guid.TryParse(createStudentDto.UserId, out var userId))
            {
                throw new ArgumentException("Invalid UserId format.");
            }

            var user = await _userRepository.GetByIdAsync(createStudentDto.UserId);
            if (user == null)
            {
                throw new Exception("User not found.");
            }

            if (user.Role != FPTStella.Domain.Enum.Role.Student)
            {
                throw new Exception("User must have role 'Student' to create a Student record.");
            }

            var existingStudent = await _studentRepository.GetByUserIdAsync(userId);
            if (existingStudent != null)
            {
                throw new Exception("A Student record already exists for this UserId.");
            }

            var studentByCode = await _studentRepository.GetByStudentCodeAsync(createStudentDto.StudentCode);
            if (studentByCode != null)
            {
                throw new Exception("StudentCode already exists.");
            }

            var student = new Student
            {
                UserId = userId,
                StudentCode = createStudentDto.StudentCode,
                Phone = createStudentDto.Phone,
                Address = createStudentDto.Address,
            };

            await _studentRepository.InsertAsync(student);

            return new StudentDto
            {
                Id = student.Id.ToString(),
                UserId = student.UserId.ToString(),
                StudentCode = student.StudentCode,
                Phone = student.Phone,
                Address = student.Address,
            };
        }
        public async Task<StudentDto> GetStudentByIdAsync(string id)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                throw new Exception("Student not found in system.");
            }

            return new StudentDto
            {
                Id = student.Id.ToString(),
                UserId = student.UserId.ToString(),
                StudentCode = student.StudentCode,
                Phone = student.Phone,
                Address = student.Address,
            };
        }
        public async Task<StudentDto> GetStudentByStudentCodeAsync(string studentCode)
        {
            var student = await _studentRepository.GetByStudentCodeAsync(studentCode);
            if (student == null)
            {
                throw new Exception("Student not found in system.");
            }

            return new StudentDto
            {
                Id = student.Id.ToString(),
                UserId = student.UserId.ToString(),
                StudentCode = student.StudentCode,
                Phone = student.Phone,
                Address = student.Address,
            };
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
                throw new Exception("Student not found for this UserId.");
            }
            return new StudentDto
            {
                Id = student.Id.ToString(),
                UserId = student.UserId.ToString(),
                StudentCode = student.StudentCode,
                Phone = student.Phone,
                Address = student.Address,
            };
        }
        public async Task UpdateStudentAsync(string id, UpdateStudentDto updateStudentDto)
        {
            var student = await _studentRepository.GetByIdAsync(id);
            if (student == null)
            {
                throw new Exception("Student not found.");
            }

            if (student.StudentCode != updateStudentDto.StudentCode)
            {
                var studentByCode = await _studentRepository.GetByStudentCodeAsync(updateStudentDto.StudentCode);
                if (studentByCode != null)
                {
                    throw new Exception("StudentCode already exists.");
                }
            }

            student.StudentCode = updateStudentDto.StudentCode;
            student.Phone = updateStudentDto.Phone;
            student.Address = updateStudentDto.Address;

            await _studentRepository.ReplaceAsync(id, student);
        }

        public async Task DeleteStudentAsync(string id)
        {
            await _studentRepository.DeleteAsync(id);
        }
    }
}

using FPTStella.Application.Common.Interfaces.Repositories;
using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Application.Utils;
using FPTStella.Contracts.DTOs.Students;
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
                MajorId = student.MajorId.ToString(),
                StudentCode = student.StudentCode,
                Phone = student.Phone,
                Address = student.Address,
            };
        }

        public async Task<StudentDto> CreateStudentAsync(CreateStudentDto createStudentDto,HttpContext http)
        {
            var studentRepository = _unitOfWork.Repository<Student>();
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

            var studentByCode = await studentRepository.FindOneAsync(s => s.StudentCode == createStudentDto.StudentCode);
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
        public async Task<List<StudentDto>> GetAllStudentsAsync()
        {
            var studentRepository = _unitOfWork.Repository<Student>();
            var students = await studentRepository.GetAllAsync();
            return students.Select(MapToStudentDto).ToList();
        }
    }
}

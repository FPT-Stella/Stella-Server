using FPTStella.Contracts.DTOs.Students;
using FPTStella.Domain.Common;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IStudentService
    {
        Task<StudentDto> CreateStudentAsync(CreateStudentDto createStudentDto, HttpContext httpContext);
        Task<StudentDto> GetCurrentStudentAsync(HttpContext httpContext);
        Task<StudentDto> GetStudentByIdAsync(string id);
        Task<StudentDto> GetStudentByStudentCodeAsync(string studentCode);
        Task<StudentDto> GetStudentByUserIdAsync(string userId);
        Task UpdateStudentAsync(string id, UpdateStudentDto updateStudentDto);
        Task DeleteStudentAsync(string id);
        Task<List<StudentDto>> GetAllStudentsAsync();
        /// <summary>
        /// Searches for students with advanced filtering options and pagination
        /// </summary>
        /// <param name="searchTerm">Optional search term for StudentCode, Phone, or Address</param>
        /// <param name="majorId">Optional major ID filter</param>
        /// <param name="pageNumber">Page number (default: 1)</param>
        /// <param name="pageSize">Page size (default: 10)</param>
        /// <returns>Paged result of students as DTOs</returns>
        Task<PagedResult<StudentDto>> SearchStudentsAsync(
            string? searchTerm = null,
            Guid? majorId = null,
            int pageNumber = 1,
            int pageSize = 10);
    }
}

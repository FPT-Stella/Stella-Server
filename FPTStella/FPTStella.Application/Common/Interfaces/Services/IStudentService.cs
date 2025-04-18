﻿using FPTStella.Contracts.DTOs.Students;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IStudentService
    {
        Task<StudentDto> CreateStudentAsync(CreateStudentDto createStudentDto);
        Task<StudentDto> GetStudentByIdAsync(string id);
        Task<StudentDto> GetStudentByStudentCodeAsync(string studentCode);
        Task<StudentDto> GetStudentByUserIdAsync(string userId);
        Task UpdateStudentAsync(string id, UpdateStudentDto updateStudentDto);
        Task DeleteStudentAsync(string id);
    }
}

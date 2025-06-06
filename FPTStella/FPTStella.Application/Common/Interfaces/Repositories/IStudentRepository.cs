﻿using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Common;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface IStudentRepository : IRepository<Student>
    {
        Task<Student?> GetByStudentCodeAsync(string studentCode);
        Task<Student?> GetByUserIdAsync(Guid userId);
        Task<PagedResult<Student>> SearchStudentsAsync(
            string? searchTerm = null,
            Guid? majorId = null,
            PaginationParams? paginationParams = null);
    }
}

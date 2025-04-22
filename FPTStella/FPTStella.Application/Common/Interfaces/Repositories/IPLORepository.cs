using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface IPLORepository : IRepository<PLOs>
    {
        /// <summary>
        /// Lấy danh sách PLOs theo Curriculum Id.
        /// </summary>
        /// <param name="curriculumId">Id của Curriculum.</param>
        /// <returns>Danh sách PLOs.</returns>
        Task<List<PLOs>> GetByCurriculumIdAsync(Guid curriculumId);
        /// <summary>
        /// Kiểm tra xem một PLO với tên cụ thể (PloName) đã tồn tại trong một Curriculum hay chưa.
        /// </summary>
        /// <param name="curriculumId"></param>
        /// <param name="ploName"></param>
        /// <returns></returns>
        Task<bool> IsPloNameExistedAsync(Guid curriculumId, string ploName);
        /// <summary>
        /// Lấy danh sách PLOs theo danh sách Curriculum Ids.
        /// </summary>
        /// <param name="curriculumIds"></param>
        /// <returns></returns>
        Task<List<PLOs>> GetByCurriculumIdsAsync(List<Guid> curriculumIds);
        /// <summary>
        /// Xóa tất cả các PLO theo Curriculum Id.
        /// </summary>
        /// <param name="curriculumId"></param>
        /// <returns></returns>
        Task DeleteByCurriculumIdAsync(Guid curriculumId);
    }
}

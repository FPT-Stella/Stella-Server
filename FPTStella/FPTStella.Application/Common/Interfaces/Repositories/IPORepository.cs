using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface IPORepository : IRepository<POs>
    {
        /// <summary>
        /// Lấy danh sách POs theo Program Id.
        /// </summary>
        /// <param name="programId">Id của Program.</param>
        /// <returns>Danh sách POs.</returns>
        Task<List<POs>> GetByProgramIdAsync(Guid programId);
        /// <summary>
        /// Kiểm tra xem một PO với tên cụ thể (PoName) đã tồn tại trong một Program hay chưa.
        /// </summary>
        /// <param name="programId"></param>
        /// <param name="poName"></param>
        /// <returns></returns>
        Task<bool> IsPoNameExistedAsync(Guid programId, string poName);
        /// <summary>
        /// Lấy danh sách POs theo danh sách Program Ids.
        /// </summary>
        /// <param name="programIds"></param>
        /// <returns></returns>
        Task<List<POs>> GetByProgramIdsAsync(List<Guid> programIds);
        /// <summary>
        /// Xóa tất cả các PO theo Program Id.
        /// </summary>
        /// <param name="programId"></param>
        /// <returns></returns>
        Task DeleteByProgramIdAsync(Guid programId);
    }
}

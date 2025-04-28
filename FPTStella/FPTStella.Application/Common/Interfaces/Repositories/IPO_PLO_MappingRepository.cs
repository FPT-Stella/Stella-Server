using FPTStella.Application.Common.Interfaces.UnitOfWorks;
using FPTStella.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Repositories
{
    public interface IPO_PLO_MappingRepository : IRepository<PO_PLO_Mapping>
    {
        /// <summary>
        /// Lấy danh sách PO_PLO_Mapping theo PO Id.
        /// </summary>
        /// <param name="poId">Id của PO.</param>
        /// <returns>Danh sách PO_PLO_Mapping.</returns>
        Task<List<PO_PLO_Mapping>> GetByPoIdAsync(Guid poId);
        /// <summary>
        /// Lấy danh sách PO_PLO_Mapping theo PLO Id.
        /// </summary>
        /// <param name="ploId">Id của PLO.</param>
        /// <returns>Danh sách PO_PLO_Mapping.</returns>
        Task<List<PO_PLO_Mapping>> GetByPloIdAsync(Guid ploId);
        /// <summary>
        ///  Kiểm tra xem một mối quan hệ giữa PO và PLO đã tồn tại hay chưa.
        ///  </summary>>
        Task<bool> IsMappingExistedAsync(Guid poId, Guid ploId);
        /// <summary>
        /// Xoá mối quan hệ giữa PO và PLO theo PO Id.
        /// <param name="poId">Id của PO.</param>  
        /// </summary>
        Task DeleteMappingsByPoIdAsync(Guid poId);
        /// <summary>
        /// Xóa mối quan hệ giữa PO và PLO theo PLO Id.
        /// <param name="ploId">Id của PO.</param>
        /// </summary>
        Task DeleteMappingsByPloIdAsync(Guid ploId);
        /// <summary>
        /// Lấy danh sách PLOs liên kết với một PO.
        /// <param name="poId">Id của PO.</param>
        /// </summary>
        Task<List<Guid>> GetPloIdsByPoIdAsync(Guid poId);
        /// <summary>
        /// Lấy danh sách POs liên kết với một PLO.
        /// <param name="ploId">Id của PO.</param>
        /// </summary>
        Task<List<Guid>> GetPoIdsByPloIdAsync(Guid ploId);
        Task<List<(Guid Id, string Name)>> GetPOsWithNameByPloIdAsync(Guid ploId);

    }
}

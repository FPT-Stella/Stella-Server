using FPTStella.Contracts.DTOs.Curriculums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface ICurriculumService
    {
        /// <summary>
        /// Tạo mới một Curriculum.
        /// </summary>
        /// <param name="createCurriculumDto">Thông tin của Curriculum cần tạo.</param>
        /// <returns>CurriculumDto chứa thông tin của Curriculum vừa tạo.</returns>
        Task<CurriculumDto> CreateCurriculumAsync(CreateCurriculumDto createCurriculumDto);

        /// <summary>
        /// Lấy thông tin Curriculum theo Id.
        /// </summary>
        /// <param name="id">Id của Curriculum.</param>
        /// <returns>CurriculumDto chứa thông tin của Curriculum.</returns>
        Task<CurriculumDto> GetCurriculumByIdAsync(Guid id);

        /// <summary>
        /// Lấy thông tin Curriculum theo mã CurriculumCode.
        /// </summary>
        /// <param name="curriculumCode">Mã CurriculumCode.</param>
        /// <returns>CurriculumDto chứa thông tin của Curriculum.</returns>
        Task<CurriculumDto> GetCurriculumByCurriculumCodeAsync(string curriculumCode);
        /// <summary>
        /// Lấy danh sách tất cả các Curriculum.
        /// </summary>
        /// <returns>Danh sách CurriculumDto.</returns>
        Task<List<CurriculumDto>> GetAllCurriculumsAsync();


        /// <summary>
        /// Lấy thông tin Curriculum theo tên CurriculumName.
        /// </summary>
        /// <param name="curriculumName">Tên CurriculumName.</param>
        /// <returns>CurriculumDto chứa thông tin của Curriculum.</returns>
        Task<CurriculumDto> GetCurriculumByCurriculumNameAsync(string curriculumName);

        /// <summary>
        /// Lấy danh sách Curriculum theo ProgramId.
        /// </summary>
        /// <param name="programId">Id của Program.</param>
        /// <returns>Danh sách CurriculumDto.</returns>
        Task<List<CurriculumDto>> GetCurriculumsByProgramIdAsync(Guid programId);

        /// <summary>
        /// Cập nhật thông tin Curriculum.
        /// </summary>
        /// <param name="id">Id của Curriculum cần cập nhật.</param>
        /// <param name="updateCurriculumDto">Thông tin cập nhật.</param>
        Task UpdateCurriculumAsync(Guid id, UpdateCurriculumDto updateCurriculumDto);

        /// <summary>
        /// Xóa Curriculum theo Id.
        /// </summary>
        /// <param name="id">Id của Curriculum cần xóa.</param>
        Task DeleteCurriculumAsync(Guid id);

        /// <summary>
        /// Kiểm tra xem CurriculumCode đã tồn tại hay chưa.
        /// </summary>
        /// <param name="curriculumCode">Mã CurriculumCode.</param>
        /// <returns>True nếu tồn tại, ngược lại False.</returns>
        Task<bool> IsCurriculumCodeExistedAsync(string curriculumCode);

        /// <summary>
        /// Kiểm tra xem CurriculumName đã tồn tại hay chưa.
        /// </summary>
        /// <param name="curriculumName">Tên CurriculumName.</param>
        /// <returns>True nếu tồn tại, ngược lại False.</returns>
        Task<bool> IsCurriculumNameExistedAsync(string curriculumName);

        /// <summary>
        /// Kiểm tra xem CurriculumCode đã tồn tại hay chưa, ngoại trừ Curriculum có Id được chỉ định.
        /// </summary>
        /// <param name="curriculumCode">Mã CurriculumCode.</param>
        /// <param name="id">Id của Curriculum cần loại trừ.</param>
        /// <returns>True nếu tồn tại, ngược lại False.</returns>
        Task<bool> IsCurriculumCodeExistedAsync(string curriculumCode, Guid id);

        /// <summary>
        /// Kiểm tra xem CurriculumName đã tồn tại hay chưa, ngoại trừ Curriculum có Id được chỉ định.
        /// </summary>
        /// <param name="curriculumName">Tên CurriculumName.</param>
        /// <param name="id">Id của Curriculum cần loại trừ.</param>
        /// <returns>True nếu tồn tại, ngược lại False.</returns>
        Task<bool> IsCurriculumNameExistedAsync(string curriculumName, Guid id);

    }
}

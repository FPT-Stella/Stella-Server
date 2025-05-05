using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Dashboard
{
    /// <summary>
    /// DTO chứa thống kê về sinh viên theo chuyên ngành
    /// </summary>
    public class StudentMajorStatisticsDto
    {
        /// <summary>
        /// Tổng số sinh viên trong hệ thống
        /// </summary>
        public int TotalStudents { get; set; }

        /// <summary>
        /// Thống kê số sinh viên theo chuyên ngành
        /// </summary>
        public List<MajorStatistics> StudentsByMajor { get; set; } = new List<MajorStatistics>();
    }
    /// <summary>
    /// Thống kê về số sinh viên theo chuyên ngành
    /// </summary>
    public class MajorStatistics
    {
        /// <summary>
        /// ID của chuyên ngành
        /// </summary>
        public string MajorId { get; set; } = string.Empty;

        /// <summary>
        /// Tên chuyên ngành
        /// </summary>
        public string MajorName { get; set; } = string.Empty;

        /// <summary>
        /// Số lượng sinh viên thuộc chuyên ngành
        /// </summary>
        public int StudentCount { get; set; }
    }
}

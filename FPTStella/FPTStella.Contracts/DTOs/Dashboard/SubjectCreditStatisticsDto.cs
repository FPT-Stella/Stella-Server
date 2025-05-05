using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Dashboard
{
    public class SubjectCreditStatisticsDto
    {
        /// <summary>
        /// Tổng số môn học trong hệ thống
        /// </summary>
        public int TotalSubjects { get; set; }

        /// <summary>
        /// Thống kê môn học theo số tín chỉ
        /// </summary>
        public List<CreditStatistics> SubjectsByCredits { get; set; } = new List<CreditStatistics>();
    }

    /// <summary>
    /// Thống kê môn học theo số tín chỉ
    /// </summary>
    public class CreditStatistics
    {
        /// <summary>
        /// Số tín chỉ
        /// </summary>
        public int Credits { get; set; }

        /// <summary>
        /// Số lượng môn học có số tín chỉ tương ứng
        /// </summary>
        public int SubjectCount { get; set; }
    }
}

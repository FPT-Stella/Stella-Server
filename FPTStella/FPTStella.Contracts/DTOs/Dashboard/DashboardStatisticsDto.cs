using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Contracts.DTOs.Dashboard
{
    public class DashboardStatisticsDto
    {
        /// <summary>
        /// Tổng số sinh viên trong hệ thống
        /// </summary>
        public int TotalStudents { get; set; }

        /// <summary>
        /// Tổng số môn học trong hệ thống
        /// </summary>
        public int TotalSubjects { get; set; }

        /// <summary>
        /// Tổng số chương trình học trong hệ thống
        /// </summary>
        public int TotalPrograms { get; set; }

        /// <summary>
        /// Tổng số chuyên ngành trong hệ thống
        /// </summary>
        public int TotalMajors { get; set; }

        /// <summary>
        /// Tổng số giáo trình trong hệ thống
        /// </summary>
        public int TotalCurriculums { get; set; }

        /// <summary>
        /// Số lượng CLO (Course Learning Outcomes) trong hệ thống
        /// </summary>
        public int TotalCLOs { get; set; }

        /// <summary>
        /// Số lượng PLO (Program Learning Outcomes) trong hệ thống
        /// </summary>
        public int TotalPLOs { get; set; }
    }
}

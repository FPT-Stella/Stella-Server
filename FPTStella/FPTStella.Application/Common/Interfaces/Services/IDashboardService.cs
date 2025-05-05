using FPTStella.Contracts.DTOs.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTStella.Application.Common.Interfaces.Services
{
    public interface IDashboardService
    {
        /// <summary>
        /// Gets the dashboard statistics for admin.
        /// </summary>
        /// <returns>The dashboard statistics.</returns>
        Task<DashboardStatisticsDto> GetDashboardStatisticsAsync();
        /// <summary>
        /// Gets statistics about students grouped by major.
        /// </summary>
        /// <returns>Student statistics by major.</returns>
        Task<StudentMajorStatisticsDto> GetStudentMajorStatisticsAsync();

        /// <summary>
        /// Gets statistics about subjects grouped by credits.
        /// </summary>
        /// <returns>Subject statistics by credits.</returns>
        Task<SubjectCreditStatisticsDto> GetSubjectCreditStatisticsAsync();
    }
}

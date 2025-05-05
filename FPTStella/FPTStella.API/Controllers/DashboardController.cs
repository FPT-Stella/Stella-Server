using FPTStella.Application.Common.Interfaces.Services;
using FPTStella.Contracts.DTOs.Dashboard;
using FPTStella.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FPTStella.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [HttpGet("statistics")]
        public async Task<ActionResult<DashboardStatisticsDto>> GetDashboardStatistics()
        {
            var statistics = await _dashboardService.GetDashboardStatisticsAsync();
            return Ok(statistics);
        }
        [HttpGet("statistics/students-by-major")]
        public async Task<ActionResult<StudentMajorStatisticsDto>> GetStudentMajorStatistics()
        {
            var statistics = await _dashboardService.GetStudentMajorStatisticsAsync();
            return Ok(statistics);
        }
        [HttpGet("statistics/subjects-by-credits")]
        public async Task<ActionResult<SubjectCreditStatisticsDto>> GetSubjectCreditStatistics()
        {
            var statistics = await _dashboardService.GetSubjectCreditStatisticsAsync();
            return Ok(statistics);
        }
    }
}

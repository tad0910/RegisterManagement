using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentInternshipManagement.Services;
using System.Threading.Tasks;

namespace StudentInternshipManagement.Controllers
{
    [Route("api/reports")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IInternshipService _internshipService;

        public ReportController(IInternshipService internshipService)
        {
            _internshipService = internshipService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetReports()
        {
            var internships = await _internshipService.GetAllInternshipsAsync();
            var reports = new
            {
                totalStudents = internships.Select(i => i.StudentId).Distinct().Count(),
                totalCompanies = internships.Select(i => i.CompanyId).Distinct().Count(),
                approvedInternships = internships.Count(i => i.Status == "Approved"),
                pendingInternships = internships.Count(i => i.Status == "Pending")
            };
            return Ok(reports);
        }
    }
}
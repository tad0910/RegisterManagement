using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentInternshipManagement.Services;

namespace StudentInternshipManagement.Controllers
{
    [Route("api/internships")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class InternshipController : ControllerBase
    {
        private readonly IInternshipService _internshipService;

        public InternshipController(IInternshipService internshipService)
        {
            _internshipService = internshipService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var internships = await _internshipService.GetAllInternshipsAsync();
                return Ok(internships);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterInternship(int studentId, int companyId)
        {
            try
            {
                await _internshipService.RegisterInternshipAsync(studentId, companyId);
                return Ok(new { Message = "Internship registered successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("approve/{internshipId}")]
        public async Task<IActionResult> ApproveInternship(int internshipId)
        {
            try
            {
                await _internshipService.ApproveInternshipAsync(internshipId);
                return Ok(new { Message = "Internship approved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("reject/{internshipId}")]
        public async Task<IActionResult> RejectInternship(int internshipId)
        {
            try
            {
                await _internshipService.RejectInternshipAsync(internshipId);
                return Ok(new { Message = "Internship rejected successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
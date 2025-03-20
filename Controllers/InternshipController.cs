using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentInternshipManagement.Services;
using System.Security.Claims;

namespace StudentInternshipManagement.Controllers
{
    [Route("api/internships")]
    [ApiController]
    public class InternshipController : ControllerBase
    {
        private readonly IInternshipService _internshipService;

        public InternshipController(IInternshipService internshipService)
        {
            _internshipService = internshipService ?? throw new ArgumentNullException(nameof(internshipService));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")] // Chỉ Admin xem danh sách tất cả thực tập
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
        [Authorize(Roles = "Student")] // Chỉ Student đăng ký
        public async Task<IActionResult> RegisterInternship(int studentId, int companyId)
        {
            try
            {
                // Lấy studentId từ token (ClaimTypes.NameIdentifier)
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new UnauthorizedAccessException("Invalid token"));
                if (userId != studentId)
                    return Unauthorized(new { Message = "Invalid student ID" });

                await _internshipService.RegisterInternshipAsync(studentId, companyId);
                return Ok(new { Message = "Internship registered successfully. Check your notifications!" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("approve/{internshipId}")]
        [Authorize(Roles = "Admin")] // Chỉ Admin duyệt
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
        [Authorize(Roles = "Admin")] // Chỉ Admin từ chối
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

        [HttpGet("my-registrations")]
        [Authorize(Roles = "Student")] // Chỉ Student xem danh sách đăng ký của mình
        public async Task<IActionResult> GetMyRegistrations()
        {
            try
            {
                // Lấy studentId từ token
                var studentId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? throw new UnauthorizedAccessException("Invalid token"));
                var internships = await _internshipService.GetInternshipsByStudentAsync(studentId);
                return Ok(internships);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
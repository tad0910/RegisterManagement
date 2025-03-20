using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentInternshipManagement.Models;
using StudentInternshipManagement.Services;

namespace StudentInternshipManagement.Controllers
{
    [Route("api/students")]
    [ApiController]
   
    public class StudentController : ControllerBase
    {
        private readonly StudentService _studentService;

        public StudentController(StudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddStudent([FromBody] Student student)
        {
            if (student == null || string.IsNullOrEmpty(student.Name))
            {
                return BadRequest(new { Message = "Student data is invalid" });
            }
            try
            {
                var newStudent = await _studentService.AddStudentAsync(student);
                return CreatedAtAction(nameof(GetAllStudents), new { id = newStudent.Id }, newStudent);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateStudent(int id, [FromBody] Student student)
        {
            if (student == null || id != student.Id)
            {
                return BadRequest(new { Message = "ID mismatch or invalid data" });
            }
            try
            {
                await _studentService.UpdateStudentAsync(student);
                return Ok(new { Message = "Student updated" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    return NotFound(new { Message = "Student not found" });
                }
                await _studentService.DeleteStudentAsync(id);
                return Ok(new { Message = "Student deleted successfully" });
                // Hoặc dùng CreatedAtAction nếu cần trả về resource mới
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            try
            {
                var students = await _studentService.GetAllStudentsAsync();
                return Ok(students);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetStudent(int id)
        {
            try
            {
                var student = await _studentService.GetStudentByIdAsync(id);
                if (student == null)
                {
                    return NotFound(new { Message = "Student not found" });
                }
                return Ok(student);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
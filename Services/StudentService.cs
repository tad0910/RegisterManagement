using Microsoft.EntityFrameworkCore;
using StudentInternshipManagement.Data;
using StudentInternshipManagement.Models;
using StudentInternshipManagement.Repositories;

namespace StudentInternshipManagement.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly INotificationService _notificationService;
        private readonly AppDbContext _context;

        public StudentService(IStudentRepository studentRepo, INotificationService notificationService, AppDbContext context)
        {
            _studentRepo = studentRepo;
            _notificationService = notificationService;
            _context = context;
        }

        // Cập nhật đăng ký thực tập: dùng companyId thay vì companyName
        public async Task RegisterInternshipAsync(int studentId, int companyId)
        {
            var student = await _studentRepo.GetByIdAsync(studentId);
            if (student == null) throw new Exception("Student not found");

            var company = await _context.Companies.FindAsync(companyId);
            if (company == null) throw new Exception("Company not found");

            var internship = new Internship { StudentId = studentId, CompanyId = companyId, Status = "Pending" };
            _context.Internships.Add(internship);
            await _context.SaveChangesAsync();

            if (!string.IsNullOrEmpty(student.Email))
            {
                await _notificationService.SendEmailAsync(student.Email, "Internship Registration", $"You have registered for an internship at {company.Name}.");
            }
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _studentRepo.GetAllAsync();
        }

        public async Task<List<Internship>> GetAllInternshipsAsync()
        {
            return await _context.Internships
                .Include(i => i.Student)
                .Include(i => i.Company)
                .ToListAsync();
        }

        public async Task ApproveInternshipAsync(int internshipId)
        {
            var internship = await _context.Internships
                .Include(i => i.Student)
                .Include(i => i.Company)
                .FirstOrDefaultAsync(i => i.Id == internshipId);
            if (internship == null) throw new Exception("Internship not found");

            internship.Status = "Approved";
            await _context.SaveChangesAsync();

            if (internship.Student != null && !string.IsNullOrEmpty(internship.Student.Email))
            {
                await _notificationService.SendEmailAsync(internship.Student.Email, "Internship Approved", $"Your internship at {internship.Company.Name} has been approved.");
            }
        }

        // Thêm phương thức từ chối đăng ký thực tập
        public async Task RejectInternshipAsync(int internshipId)
        {
            var internship = await _context.Internships
                .Include(i => i.Student)
                .Include(i => i.Company)
                .FirstOrDefaultAsync(i => i.Id == internshipId);
            if (internship == null) throw new Exception("Internship not found");

            internship.Status = "Rejected";
            await _context.SaveChangesAsync();

            if (internship.Student != null && !string.IsNullOrEmpty(internship.Student.Email))
            {
                await _notificationService.SendEmailAsync(internship.Student.Email, "Internship Rejected", $"Your internship at {internship.Company.Name} has been rejected.");
            }
        }

        public async Task<Student> AddStudentAsync(Student student)
        {
            if (string.IsNullOrWhiteSpace(student.Name) ||
                string.IsNullOrWhiteSpace(student.StudentCode) ||
                string.IsNullOrWhiteSpace(student.Email))
            {
                throw new ArgumentException("All fields (Name, StudentCode, Email) are required.");
            }

            var existingStudent = await _context.Students
                .FirstOrDefaultAsync(s => s.StudentCode == student.StudentCode);
            if (existingStudent != null)
            {
                throw new Exception("StudentCode already exists.");
            }

            await _studentRepo.AddAsync(student);
            return student;
        }

        public async Task UpdateStudentAsync(Student student)
        {
            var existingStudent = await _studentRepo.GetByIdAsync(student.Id);
            if (existingStudent == null) throw new Exception("Student not found");

            existingStudent.Name = student.Name;
            existingStudent.StudentCode = student.StudentCode;
            existingStudent.Email = student.Email;
            await _studentRepo.UpdateAsync(existingStudent);
        }

        public async Task DeleteStudentAsync(int id)
        {
            var student = await _studentRepo.GetByIdAsync(id);
            if (student == null) throw new Exception("Student not found");

            await _studentRepo.DeleteAsync(id);
        }
        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.FindAsync(id);
        }
    }
}
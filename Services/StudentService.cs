using StudentInternshipManagement.Models;
using StudentInternshipManagement.Repositories;

namespace StudentInternshipManagement.Services
{
    public class StudentService
    {
        private readonly IStudentRepository _studentRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly IInternshipRepository _internshipRepo;
        private readonly INotificationService _notificationService;

        public StudentService(
            IStudentRepository studentRepo,
            ICompanyRepository companyRepo,
            IInternshipRepository internshipRepo,
            INotificationService notificationService)
        {
            _studentRepo = studentRepo;
            _companyRepo = companyRepo;
            _internshipRepo = internshipRepo;
            _notificationService = notificationService;
        }

        public async Task RegisterInternshipAsync(int studentId, int companyId)
        {
            var student = await _studentRepo.GetByIdAsync(studentId);
            if (student == null) throw new Exception("Student not found");

            var company = await _companyRepo.GetByIdAsync(companyId);
            if (company == null) throw new Exception("Company not found");

            var internship = new Internship { StudentId = studentId, CompanyId = companyId, Status = "Pending" };
            await _internshipRepo.AddAsync(internship);

            if (!string.IsNullOrEmpty(student.Email))
            {
                await _notificationService.SendEmailAsync(
                    student.Email,
                    "Internship Registration",
                    $"You have registered for an internship at {company.Name}.");
            }
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _studentRepo.GetAllAsync();
        }

        public async Task<List<Internship>> GetAllInternshipsAsync()
        {
            return await _internshipRepo.GetAllAsync();
        }

        public async Task ApproveInternshipAsync(int internshipId)
        {
            var internship = await _internshipRepo.GetByIdAsync(internshipId);
            if (internship == null) throw new Exception("Internship not found");

            internship.Status = "Approved";
            await _internshipRepo.UpdateAsync(internship);

            if (internship.Student != null && !string.IsNullOrEmpty(internship.Student.Email))
            {
                await _notificationService.SendEmailAsync(
                    internship.Student.Email,
                    "Internship Approved",
                    $"Your internship at {internship.Company.Name} has been approved.");
            }
        }

        public async Task RejectInternshipAsync(int internshipId)
        {
            var internship = await _internshipRepo.GetByIdAsync(internshipId);
            if (internship == null) throw new Exception("Internship not found");

            internship.Status = "Rejected";
            await _internshipRepo.UpdateAsync(internship);

            if (internship.Student != null && !string.IsNullOrEmpty(internship.Student.Email))
            {
                await _notificationService.SendEmailAsync(
                    internship.Student.Email,
                    "Internship Rejected",
                    $"Your internship at {internship.Company.Name} has been rejected.");
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

            var existingStudent = await _studentRepo.GetByStudentCodeAsync(student.StudentCode);
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
            if (student == null)
            {
                throw new Exception("Student not found");
            }
            await _studentRepo.DeleteAsync(id);
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            var student = await _studentRepo.GetByIdAsync(id);
            if (student == null) throw new Exception("Student not found");
            return student;
        }
    }
}
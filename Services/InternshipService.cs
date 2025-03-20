using StudentInternshipManagement.Models;
using StudentInternshipManagement.Repositories;

namespace StudentInternshipManagement.Services
{
    public class InternshipService : IInternshipService
    {
        private readonly IInternshipRepository _internshipRepo;
        private readonly IStudentRepository _studentRepo;
        private readonly ICompanyRepository _companyRepo;
        private readonly INotificationService _notificationService;

        public InternshipService(
            IInternshipRepository internshipRepo,
            IStudentRepository studentRepo,
            ICompanyRepository companyRepo,
            INotificationService notificationService)
        {
            _internshipRepo = internshipRepo;
            _studentRepo = studentRepo;
            _companyRepo = companyRepo;
            _notificationService = notificationService;
        }

        public async Task<List<Internship>> GetAllInternshipsAsync()
        {
            return await _internshipRepo.GetAllAsync();
        }

        public async Task RegisterInternshipAsync(int studentId, int companyId)
        {
            var student = await _studentRepo.GetByIdAsync(studentId);
            if (student == null) throw new Exception("Student not found");

            var company = await _companyRepo.GetByIdAsync(companyId);
            if (company == null) throw new Exception("Company not found");

            var internship = new Internship
            {
                StudentId = studentId,
                CompanyId = companyId,
                Status = "Pending"
            };
            await _internshipRepo.AddAsync(internship);

            if (!string.IsNullOrEmpty(student.Email))
            {
                await _notificationService.SendEmailAsync(
                    student.Email,
                    "Internship Registration",
                    $"You have registered for an internship at {company.Name}.");
            }
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

        // Thêm phương thức để Sinh viên xem công ty đang đăng ký
        public async Task<List<Internship>> GetInternshipsByStudentAsync(int studentId)
        {
            var student = await _studentRepo.GetByIdAsync(studentId);
            if (student == null) throw new Exception("Student not found");

            return await _internshipRepo.GetByStudentIdAsync(studentId);
        }
    }
}
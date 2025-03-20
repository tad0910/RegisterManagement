using StudentInternshipManagement.Models;

namespace StudentInternshipManagement.Services
{
    public interface IInternshipService
    {
        Task<List<Internship>> GetAllInternshipsAsync();
        Task RegisterInternshipAsync(int studentId, int companyId);
        Task ApproveInternshipAsync(int internshipId);
        Task RejectInternshipAsync(int internshipId);
        Task<List<Internship>> GetInternshipsByStudentAsync(int studentId);
    }
}
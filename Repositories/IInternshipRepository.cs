using StudentInternshipManagement.Models;

namespace StudentInternshipManagement.Repositories
{
    public interface IInternshipRepository
    {
        Task<List<Internship>> GetAllAsync();
        Task<Internship?> GetByIdAsync(int id);
        Task AddAsync(Internship internship);
        Task UpdateAsync(Internship internship);
        Task<List<Internship>> GetByStudentIdAsync(int studentId);
    }
}

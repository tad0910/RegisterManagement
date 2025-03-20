using StudentInternshipManagement.Models;

namespace StudentInternshipManagement.Repositories
{
    public interface IStudentRepository
    {
        Task<List<Internship>> GetInternshipsByStudentIdAsync(int studentId);
        Task<List<Student>> GetAllAsync();
        Task<Student?> GetByIdAsync(int id);
        Task AddAsync(Student student);
        Task UpdateAsync(Student student);
        Task DeleteAsync(int id);
        Task<Student?> GetByStudentCodeAsync(string studentCode);
    }
}
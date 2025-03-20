using StudentInternshipManagement.Data;
using StudentInternshipManagement.Models;

namespace StudentInternshipManagement.Repositories
{
    public interface ICompanyRepository
    {
        Task<List<Company>> GetAllAsync();
        Task<Company?> GetByIdAsync(int id);
        Task AddAsync(Company company);
        Task UpdateAsync(Company company);
        Task DeleteAsync(int id);
        Task<Company?> GetByNameAsync(string name); // Kiểm tra công ty theo tên
        Task<List<Company>> SearchAsync(string keyword); // Tìm kiếm công ty
    }
}
using Microsoft.EntityFrameworkCore;
using StudentInternshipManagement.Data;
using StudentInternshipManagement.Models;

namespace StudentInternshipManagement.Repositories
{
    public class InternshipRepository : IInternshipRepository
    {
        private readonly AppDbContext _context;

        public InternshipRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Internship>> GetAllAsync()
        {
            return await _context.Internships
                .Include(i => i.Student)
                .Include(i => i.Company)
                .ToListAsync();
        }

        public async Task<Internship?> GetByIdAsync(int id)
        {
            return await _context.Internships
                .Include(i => i.Student)
                .Include(i => i.Company)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task AddAsync(Internship internship)
        {
            _context.Internships.Add(internship);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Internship internship)
        {
            _context.Internships.Update(internship);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Internship>> GetByStudentIdAsync(int studentId)
        {
            return await _context.Internships
                .Where(i => i.StudentId == studentId)
                .Include(i => i.Company)
                .ToListAsync();
        }
    }
}

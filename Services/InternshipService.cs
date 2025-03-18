using Microsoft.EntityFrameworkCore;
using StudentInternshipManagement.Data;
using StudentInternshipManagement.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StudentInternshipManagement.Services
{
    public class InternshipService : IInternshipService
    {
        private readonly AppDbContext _context;

        public InternshipService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Internship>> GetAllInternshipsAsync()
        {
            return await _context.Internships
                .Include(i => i.Student)
                .Include(i => i.Company)
                .ToListAsync();
        }

        public async Task RegisterInternshipAsync(int studentId, int companyId)
        {
            var internship = new Internship
            {
                StudentId = studentId,
                CompanyId = companyId,
                Status = "Pending"
            };
            _context.Internships.Add(internship);
            await _context.SaveChangesAsync();
        }

        public async Task ApproveInternshipAsync(int internshipId)
        {
            var internship = await _context.Internships
                .FirstOrDefaultAsync(i => i.Id == internshipId);
            if (internship == null)
            {
                throw new Exception("Internship not found");
            }
            internship.Status = "Approved";
            _context.Internships.Update(internship);
            await _context.SaveChangesAsync();
        }

        public async Task RejectInternshipAsync(int internshipId)
        {
            var internship = await _context.Internships
                .FirstOrDefaultAsync(i => i.Id == internshipId);
            if (internship == null)
            {
                throw new Exception("Internship not found");
            }
            internship.Status = "Rejected";
            _context.Internships.Update(internship);
            await _context.SaveChangesAsync();
        }
    }
}
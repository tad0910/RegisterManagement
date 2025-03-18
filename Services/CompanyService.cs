using Microsoft.EntityFrameworkCore;
using StudentInternshipManagement.Data;
using StudentInternshipManagement.Models;
using StudentInternshipManagement.Repositories;

namespace StudentInternshipManagement.Services
{
    public class CompanyService
    {
        private readonly ICompanyRepository _companyRepo;
        private readonly AppDbContext _context;

        public CompanyService(ICompanyRepository companyRepo, AppDbContext context)
        {
            _companyRepo = companyRepo;
            _context = context;
        }

        public async Task<List<Company>> GetAllCompaniesAsync()
        {
            return await _companyRepo.GetAllAsync();
        }

        public async Task<Company> GetCompanyByIdAsync(int id)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null) throw new Exception("Company not found");
            return company;
        }

        public async Task<Company> AddCompanyAsync(Company company)
        {
            if (string.IsNullOrWhiteSpace(company.Name))
            {
                throw new ArgumentException("Company name is required.");
            }

            var existingCompany = await _context.Companies
                .FirstOrDefaultAsync(c => c.Name == company.Name);
            if (existingCompany != null)
            {
                throw new Exception("Company already exists.");
            }

            await _companyRepo.AddAsync(company);
            return company;
        }

        public async Task UpdateCompanyAsync(Company company)
        {
            var existingCompany = await _companyRepo.GetByIdAsync(company.Id);
            if (existingCompany == null) throw new Exception("Company not found");

            existingCompany.Name = company.Name;
            await _companyRepo.UpdateAsync(existingCompany);
        }

        public async Task DeleteCompanyAsync(int id)
        {
            var company = await _companyRepo.GetByIdAsync(id);
            if (company == null) throw new Exception("Company not found");

            await _companyRepo.DeleteAsync(id);
        }
    }
}
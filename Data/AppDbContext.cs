using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore; // Thêm namespace cho IdentityDbContext
using Microsoft.EntityFrameworkCore;
using StudentInternshipManagement.Models;

namespace StudentInternshipManagement.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public DbSet<Student> Students { get; set; }
        public DbSet<Internship> Internships { get; set; }
        public DbSet<Company> Companies { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Gọi OnModelCreating của IdentityDbContext để đảm bảo cấu hình Identity
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Internship>()
                .HasOne(i => i.Company)
                .WithMany()
                .HasForeignKey(i => i.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Internship>()
                .HasOne(i => i.Student)
                .WithMany()
                .HasForeignKey(i => i.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
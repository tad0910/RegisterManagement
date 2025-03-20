using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
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

            // Cấu hình quan hệ cho Internship với Company (không chỉ định navigation property trong WithMany)
            modelBuilder.Entity<Internship>()
                .HasOne(i => i.Company)
                .WithMany() // Không khai báo navigation property
                .HasForeignKey(i => i.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            // Cấu hình quan hệ cho Internship với Student (không chỉ định navigation property trong WithMany)
            modelBuilder.Entity<Internship>()
                .HasOne(i => i.Student)
                .WithMany() // Không khai báo navigation property
                .HasForeignKey(i => i.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // Thêm index cho Foreign Key để tối ưu hiệu năng
            modelBuilder.Entity<Internship>()
                .HasIndex(i => i.StudentId);

            modelBuilder.Entity<Internship>()
                .HasIndex(i => i.CompanyId);

            // Thêm ràng buộc dữ liệu
            modelBuilder.Entity<Company>()
                .Property(c => c.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Student>()
                .Property(s => s.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Student>()
                .Property(s => s.Email)
                .HasMaxLength(255);

            modelBuilder.Entity<Internship>()
                .Property(i => i.Status)
                .HasMaxLength(20);
            modelBuilder.Entity<Student>()
                .HasOne<IdentityUser>()
                .WithMany()
                .HasForeignKey(s => s.UserId);
            modelBuilder.Entity<Company>()
                .Property(c => c.Address)
                 .HasMaxLength(255);
            modelBuilder.Entity<Company>()
                .Property(c => c.Description)
                .HasMaxLength(500);
        }
    }
}
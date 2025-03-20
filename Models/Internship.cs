using StudentInternshipManagement.Data;

namespace StudentInternshipManagement.Models
{
    public class Internship
    {
        public int Id { get; set; }
        public int StudentId { get; set; }
        public Student? Student { get; set; }
        public int CompanyId { get; set; }
        public Company? Company { get; set; }
        public string Status { get; set; }
    }
}
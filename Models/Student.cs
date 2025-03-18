namespace StudentInternshipManagement.Models
{
    public class Student
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string StudentCode { get; set; }
        public required string Email { get; set; }
    }

}
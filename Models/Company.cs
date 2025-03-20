namespace StudentInternshipManagement.Models
{
    public class Company
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
    }
}
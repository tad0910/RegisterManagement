namespace StudentInternshipManagement.Services
{
    public interface INotificationService
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}

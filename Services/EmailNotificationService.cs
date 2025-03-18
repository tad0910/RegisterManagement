using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Mail;

namespace StudentInternshipManagement.Services
{
    public class EmailNotificationService : INotificationService
    {
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(ILogger<EmailNotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            _logger.LogInformation("Sending email to {Email}: Subject: {Subject}, Message: {Message}",
                email, subject, message);
            return Task.CompletedTask;
        }
    }
}

using clout.Interface;
using clout.Model;
using System.Net.Mail;

namespace clout.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpClient _smtpClient;

        public EmailService()
        {
            _smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("your-email@gmail.com", "your-email-password"),
                EnableSsl = true
            };
        }




        public void SendEmailAsync(EmailModel emailModel)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("bloger-email@example.com"),
                Subject = emailModel.Subject,
                Body = emailModel.Message,
                IsBodyHtml = true // If you want to send HTML email
            };

            mailMessage.To.Add(emailModel.To);

            _smtpClient.Send(mailMessage);

        }
    }
}

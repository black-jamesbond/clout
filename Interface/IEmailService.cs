using clout.Model;
namespace clout.Interface
{
    public interface IEmailService
    {
        void SendEmailAsync(EmailModel emailModel);
    }
}

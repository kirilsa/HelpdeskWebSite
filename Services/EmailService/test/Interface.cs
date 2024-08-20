using HelpDeskWebSite.Models;

namespace HelpDeskWebSite.Services.EmailService.test
{
    public interface IEmailService
    {
        void SendEmail(EmailDTO request);
    }
}

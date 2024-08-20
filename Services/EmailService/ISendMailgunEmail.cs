using RestSharp;

namespace HelpDeskWebSite.Services.EmailService
{
    public interface ISendMailgunEmail
    {
        RestResponse SendSimpleMessage(string to, string subject, string text);
    }
}

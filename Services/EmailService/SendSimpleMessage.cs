using HelpDeskWebSite.Models;
using Org.BouncyCastle.Asn1.Cmp;
using Org.BouncyCastle.Asn1.Crmf;
using RestSharp;
using RestSharp.Authenticators;

namespace HelpDeskWebSite.Services.EmailService
{

    public class SendMailgunEmail : ISendMailgunEmail
    {
        private readonly IConfiguration configuration;
        public SendMailgunEmail(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public RestResponse SendSimpleMessage(string to, string subject, string text)
        {
            var options = new RestClientOptions("https://api.mailgun.net/v3")
            {
                Authenticator = new HttpBasicAuthenticator("api", configuration["MailgunAPIKey"])
            };
            var client = new RestClient(options);

            var request = new RestRequest("{domain}/messages", Method.Post);
            request.AddParameter("domain", "sandboxc616e3a259d5413b964bf4d5c34f7001.mailgun.org", ParameterType.UrlSegment);
            request.AddParameter("from", "Excited User <one@solominskyi.uk>");
            request.AddParameter("to", to);
            request.AddParameter("subject", subject);
            request.AddParameter("text", text);

            return client.Execute(request);
        }
    }
}

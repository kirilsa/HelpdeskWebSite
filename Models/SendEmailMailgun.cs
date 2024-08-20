using Microsoft.AspNetCore.Mvc;

namespace HelpDeskWebSite.Models
{
    public class SendEmailMailgun
    {
        [BindProperty]
        public string To { get; set; }
        [BindProperty]
        public string Subject { get; set; }
        [BindProperty]
        public string Text { get; set; }
    }
}

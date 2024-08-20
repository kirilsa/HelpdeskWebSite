using HelpDeskWebSite.Data;
using HelpDeskWebSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SQLitePCL;

namespace HelpDeskWebSite.Controllers
{
    public class EmailMailgunController : Controller
    {
        private readonly ISendMailgunEmail _sendMailgunEmail;
        private readonly ApplicationDbContext _context;
        public EmailMailgunController(ISendMailgunEmail sendMailgunEmail, ApplicationDbContext context)
        {
            _sendMailgunEmail = sendMailgunEmail;
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(SendEmailMailgun model)
        {
            if (ModelState.IsValid)
            {
                // Call the SendSimpleMessage method on your service
                var response = _sendMailgunEmail.SendSimpleMessage(model.To, model.Subject, model.Text);

                if (response.IsSuccessful)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error sending email: " + response.ErrorMessage);
                }
            }
            return RedirectToAction("Home", "Privacy");
        }
    }
}

using HelpDeskWebSite.Data;
using HelpDeskWebSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
                    //save sent email to db
                    EmailMessage emailMessage = new EmailMessage()
                    {
                        Recipient = model.To,
                        Subject = model.Subject,
                        From = "Existing User Temp <one@solominskyi.uk>",
                        StrippedHtml = model.Text
                    };
                    DateTimeOffset Date = DateTimeOffset.Now;
                    emailMessage.Date = Date;

                    _context.Add(emailMessage);
                    await _context.SaveChangesAsync();

                    //save info to Conversation table 
                    string subject = emailMessage.Subject;
                    int emailConversationId;

                    if (subject != null && subject.StartsWith("[ID") && int.TryParse(subject.Substring(3, 4), out emailConversationId))
                    {
                        var entity = await _context.Conversations.SingleOrDefaultAsync(e => e.ConversationId == emailConversationId);

                        if (entity != null)
                        {
                            entity.UsersConversation += ",0" + emailMessage.Id;

                            _context.Conversations.Update(entity);
                        }
                    }
                    else
                    {
                        var entity = new Conversation
                        {
                            UsersConversation = "0" + emailMessage.Id.ToString()
                        };

                        _context.Conversations.Add(entity);
                    }

                    await _context.SaveChangesAsync();

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

using HelpDeskWebSite.Data;
using HelpDeskWebSite.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Text.RegularExpressions;

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
                    DateTimeOffset Date = DateTimeOffset.Now;
                    EmailMessage emailMessage = new EmailMessage()
                    {
                        Recipient = model.To,
                        Subject = model.Subject,
                        From = "Existing User Temp <one@solominskyi.uk>",
                        StrippedHtml = model.Text,
                        Date = Date,
                        EmailType = "sent"
                    };
                    _context.Add(emailMessage);
                    await _context.SaveChangesAsync();

                    //Keep track of the Users conversation
                    string subject = emailMessage.Subject;
                    int emailConversationId;
                    int IdOfTheNewEmail = emailMessage.Id;

                    //if the db already has users request with this ID
                    if (subject != null && subject.StartsWith("[ID") && int.TryParse(subject.Substring(3, 1), out emailConversationId))
                    {
                        int currentRequestId = int.Parse(Regex.Replace(subject, "[^0-9]", ""));
                        int? previousEmialIDOfConversation = await _context.ListOfRequests
                            .Where(i => i.ListOfRequestsID == currentRequestId).Select(i => i.TailOfConversation).FirstOrDefaultAsync();
                        var emailEntityOfPrevEmail = await _context.EmailMessages.SingleOrDefaultAsync(e => e.Id == previousEmialIDOfConversation);

                        if (emailEntityOfPrevEmail != null)
                        {
                            emailEntityOfPrevEmail.InReply = IdOfTheNewEmail;
                            _context.EmailMessages.Update(emailEntityOfPrevEmail);
                        }

                        var entityOfListOfRequests = await _context.ListOfRequests
                           .SingleOrDefaultAsync(i => i.ListOfRequestsID == currentRequestId);
                        entityOfListOfRequests.TailOfConversation = IdOfTheNewEmail;

                        _context.ListOfRequests.Update(entityOfListOfRequests);

                        await _context.SaveChangesAsync();

                    }
                    else
                    {
                        return RedirectToAction("Home", "Privacy");
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

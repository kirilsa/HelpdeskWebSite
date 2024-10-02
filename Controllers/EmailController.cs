using HelpDeskWebSite.Data;
using HelpDeskWebSite.Data.Migrations;
using HelpDeskWebSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System.Text.RegularExpressions;

namespace HelpDeskWebSite.Controllers
{
/*    [Route("api/[controller]")]
    [ApiController]*/
    public class EmailController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;
        //        private readonly IEmailReceiverService _emailReceiverService;

        public EmailController(IEmailService emailService, ApplicationDbContext context)
        {
            _emailService = emailService;
            _context = context;

        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id ,Recipient,Sender,From,Subject,BodyPlain,StrippedText,StrippedSignature,BodyHtml,StrippedHtml,AttachmentCount,Timestamp,Token,Signature,MessageHeaders,ContentIdMap, Date, InReply, Status, EmailType")] EmailMessage emailMessage)
        {
            //Save a new email to db
            if (ModelState.IsValid)
            {
                DateTimeOffset Date = DateTimeOffset.Now;
                emailMessage.Date = Date;
                emailMessage.EmailType = "received";
                emailMessage.From = emailMessage.From.Split('<')[0].Trim();
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
                    int previousEmialIDOfConversation = await _context.ListOfRequests
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
                //no ID in the subject
                else
                {
                    ListOfRequests newListOfRequest = new ListOfRequests
                    {
                        ListOfRequestsID = IdOfTheNewEmail,
                        HeadOfConversation = IdOfTheNewEmail,
                        TailOfConversation = IdOfTheNewEmail
                    };

                    _context.ListOfRequests.Add(newListOfRequest);
                    await _context.SaveChangesAsync();

                    subject = "[ID" + newListOfRequest.ListOfRequestsID.ToString() + "]" + subject;

                    var currentEmail = _context.EmailMessages.SingleOrDefault(i => i.Id == IdOfTheNewEmail);
                    currentEmail.Subject = subject;

                    _context.EmailMessages.Update(currentEmail);
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();
                return Ok(emailMessage);
            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage); // Or log it to a file or return as response
                }

                // Optionally return a detailed error response
                return BadRequest(ModelState);
            }

            return BadRequest("something");
        }
    }
}

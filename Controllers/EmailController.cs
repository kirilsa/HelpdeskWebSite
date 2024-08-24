using HelpDeskWebSite.Data;
using HelpDeskWebSite.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Create([Bind("Id ,Recipient,Sender,From,Subject,BodyPlain,StrippedText,StrippedSignature,BodyHtml,StrippedHtml,AttachmentCount,Timestamp,Token,Signature,MessageHeaders,ContentIdMap, Date")] EmailMessage emailMessage)
        {
            if (ModelState.IsValid)
            {
                DateTimeOffset Date = DateTimeOffset.Now;
                emailMessage.Date = Date;
                _context.Add(emailMessage);
                await _context.SaveChangesAsync();

                string subject = emailMessage.Subject;
                int emailConversationId;

                if (subject != null && subject.StartsWith("[ID") && int.TryParse(subject.Substring(3, 4), out emailConversationId))
                {
                    var entity = await _context.Conversations.SingleOrDefaultAsync(e => e.ConversationId == emailConversationId);

                    if (entity != null)
                    {
                        entity.UsersConversation += "," + emailMessage.Id;

                        _context.Conversations.Update(entity);
                    }
                }
                else
                {
                    var entity = new Conversation
                    {
                        UsersConversation = emailMessage.Id.ToString()
                    };

                    _context.Conversations.Add(entity);
                }

                await _context.SaveChangesAsync();
                return Ok(emailMessage);
            }

            return BadRequest();
        }
    }
}

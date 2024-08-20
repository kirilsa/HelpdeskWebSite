using MailKit.Net.Imap;
using MailKit;
using MailKitSimplified.Receiver.Services;
using System.Threading;
using MailKitSimplified.Receiver.Abstractions;
using HelpDeskWebSite.Models;
using MailKit.Search;
using HelpDeskWebSite.Data;

namespace HelpDeskWebSite.Services.EmailService
{
    public class EmailReceiverService : IEmailReceiverService
    {
        private readonly ApplicationDbContext _context;
        public EmailReceiverService(ApplicationDbContext context)
        {
            _context = context;
        }

        /*public EmailReceiver() 
        {
            try
            {
                using var imapClient = new ImapClient();
                imapClient.Connect("imap.gmail.com", 993, true);
                imapClient.Authenticate("kyrylsolom@gmail.com", "ypfm jlcm bgkz bkhg");

                var inbox = imapClient.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                for (int i = 0; i < Math.Min(10, inbox.Count); i++)
                {
                    var message = inbox.GetMessage(i);
                    Console.WriteLine($"Subject: {message.Subject}");
                }

                imapClient.Disconnect(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }*/

        public async Task EmailReceiver()
        {
            /*try
            {
                using var imapReceiver = ImapReceiver.Create("imap.gmail.com:993")
                .SetCredential("kyrylsolom@gmail.com", "nyix trwi lkip ajer")
                .SetProtocolLog("Logs/ImapClient.txt")
                .SetFolder("INBOX");            
                
                var mimeMessages = await imapReceiver.ReadMail.Top(1).GetMimeMessagesAsync();

                foreach (var mimeMessage in mimeMessages)
                {
                    Console.WriteLine($"Subject: {mimeMessage.Subject}");
                }
                }
                catch (Exception ex)
                {   
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
        }*/


            /*using (var client = new ImapClient())
            {
                client.Connect("imap.gmail.com", 993, true);

                client.Authenticate("kyrylsolom@gmail.com", "nyix trwi lkip ajer");

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var uids = inbox.Search(SearchQuery.NotSeen); // Retrieve unseen emails
                var messages = inbox.Fetch(uids, MessageSummaryItems.Full | MessageSummaryItems.UniqueId);

                foreach (var message in messages)
                {
                    var email = new Email
                    {
                        Sender = message.Envelope.From.ToString(),
                        Recipient = message.Envelope.To.ToString(),
                        Subject = message.Envelope.Subject,
                        Body = inbox.GetMessage(message.UniqueId).TextBody,
                        ReceivedDate = message.Date.DateTime
                    };

                    context.Emails.Add(email);
                }
                context.SaveChanges();
            }

            client.Disconnect(true);*/
        }
    }
}

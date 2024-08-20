using System.ComponentModel.DataAnnotations;

namespace HelpDeskWebSite.Models
{
    public class Conversation
    {
        [Key]
        public int ConversationId { get; set; }
        public string UsersConversation { get; set; }

    }
}

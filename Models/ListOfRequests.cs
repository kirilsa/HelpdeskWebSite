using System.ComponentModel.DataAnnotations;

namespace HelpDeskWebSite.Models
{
    public class ListOfRequests
    {
        [Key]
        public int HeadOfConversation {  get; set; }
    }
}

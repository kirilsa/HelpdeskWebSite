using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace HelpDeskWebSite.Models
{
    public class ListOfRequests
    {
        [ForeignKey("EmailMessage")]
        public int? ListOfRequestsID { get; set; }
        public int HeadOfConversation { get; set; }
        public int TailOfConversation { get; set; }
        public string Status { get; set; }
        public string? Resolution { get; set; }

        [JsonIgnore]
        public virtual EmailMessage EmailMessage { get; set; }

    }
}

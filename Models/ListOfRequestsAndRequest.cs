namespace HelpDeskWebSite.Models
{
    public class ListOfRequestsAndRequest
    {
        public IEnumerable<ListOfRequests> Data { get; set; }
        
        public List<EmailMessage> emailMessages { get; set; }
        public List<ListOfRequests> listOfRequests { get; set; }  
    }
}

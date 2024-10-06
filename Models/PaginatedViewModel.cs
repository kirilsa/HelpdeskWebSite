namespace HelpDeskWebSite.Models
{
    public class PaginatedViewModel
    {
        public IEnumerable<ListOfRequests> Data { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}

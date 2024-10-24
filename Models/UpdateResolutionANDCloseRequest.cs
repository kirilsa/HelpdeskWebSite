using Microsoft.AspNetCore.Mvc;

namespace HelpDeskWebSite.Models
{
    public class UpdateResolutionANDCloseRequest
    {
        [BindProperty]
        public string UpdatedResolution { get; set; }

        [BindProperty]
        public int RequestID { get; set; }
    }
}

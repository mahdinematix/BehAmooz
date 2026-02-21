using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Areas.Professor.Pages.Session
{
    public class WatchVideoModel : PageModel
    {
        public string VideoUrl { get; set; }

        public void OnGet(string videoUrl)
        {
            VideoUrl = videoUrl;
        }
    }
}

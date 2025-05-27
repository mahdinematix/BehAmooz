using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ServiceHost.Pages
{
    public class LoginModel : PageModel
    {
        [TempData] public string LoginMessage { get; set; }
        public void OnGet()
        {
        }
    }
}
